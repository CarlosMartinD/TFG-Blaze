using System;
using System.Collections.Generic;

public class MovementEngine
{

    private static Comparer<int> descendingComparer;

    private static int[][] variation;

    private MapEngine mapEngine;

    public MovementEngine()
    {
        mapEngine = EngineDependencyInjector.getInstance().Resolve<MapEngine>();
        descendingComparer = Comparer<int>.Create((x, y) => y.CompareTo(x));
        variation = new int[][] { new int[] { 0, -1 }, new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 } };
    }

    public ISet<Tile> GetTilesOnRange(Tile from, int range)
    {

        HashSet<Tile> visitedSquares = new HashSet<Tile>{};
        PriorityQueue<Tile> toVisit = new PriorityQueue<Tile>(descendingComparer);
        toVisit.Enqueue(from, range);
        MoveIntoNeighbourds(visitedSquares, toVisit, tile => true);
        return visitedSquares;
    }

    public ISet<Tile> GetTilesOnRange(Tile from, int range, Func<Tile, bool> conditionToVisit)
    {

        HashSet<Tile> visitedSquares = new HashSet<Tile> { };
        PriorityQueue<Tile> toVisit = new PriorityQueue<Tile>(descendingComparer);
        toVisit.Enqueue(from, range);
        MoveIntoNeighbourds(visitedSquares, toVisit, conditionToVisit);
        return visitedSquares;
    }

    private void MoveIntoNeighbourds(ISet<Tile> visitedSquares, PriorityQueue<Tile> toVisit, Func<Tile, bool> conditionToVisit)
    {
        int maximunX = mapEngine.mapWidth;
        int maximunY = mapEngine.mapHeight;
        Tile[,] map = mapEngine.mapMatrix;

        while (!toVisit.IsEmpty())
        {
            KeyValuePair <int, Tile> visiting = toVisit.DequeueWithPrio();
            visitedSquares.Add(visiting.Value);

            if(visiting.Key <= 0)
            {
                continue;
            }

            foreach (int[] movement in variation)
            {
                int newX = visiting.Value.x + movement[0];
                int newY = visiting.Value.y + movement[1];

                bool checkBoundariesX = newX >= 0 && newX < maximunX;
                bool checkBoundariesY = newY >= 0 && newY < maximunY;

                bool passToNextTile = !checkBoundariesX || !checkBoundariesY || visitedSquares.Contains(map[newX, newY]) || !conditionToVisit.Invoke(map[newX, newY]);
                if (passToNextTile)
                {
                    continue;
                }

                toVisit.Enqueue(map[newX, newY], visiting.Key - 1);
            }
        }
    }
}
