using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;

public class MovementEngine
{

    private static MovementEngine movementEngine;

    private static Comparer<int> descendingComparer;

    private static int[][]  variation;
    private MovementEngine()
    {
        descendingComparer = Comparer<int>.Create((x, y) => y.CompareTo(x));
        variation = new int[][] { new int[] { 0, -1 }, new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 } };
    }

    public static MovementEngine GetInstance()
    {
        if(movementEngine == null) 
        {
            movementEngine = new MovementEngine();
        }

        return movementEngine;
    }

    public ISet<Tile> GetTilesOnRange(Tile from, int range)
    {

        HashSet<Tile> visitedSquares = new HashSet<Tile>{};
        PriorityQueue<Tile> toVisit = new PriorityQueue<Tile>(descendingComparer);
        toVisit.Enqueue(from, range);
        MoveIntoNeighbourds(visitedSquares, toVisit, tile => true);
        visitedSquares.Remove(from);
        return visitedSquares;
    }

    public ISet<Tile> GetTilesOnRange(Tile from, int range, Func<Tile, bool> conditionToVisit)
    {

        HashSet<Tile> visitedSquares = new HashSet<Tile> { };
        PriorityQueue<Tile> toVisit = new PriorityQueue<Tile>(descendingComparer);
        toVisit.Enqueue(from, range);
        MoveIntoNeighbourds(visitedSquares, toVisit, conditionToVisit);
        visitedSquares.Remove(from);
        return visitedSquares;
    }

    private void MoveIntoNeighbourds(ISet<Tile> visitedSquares, PriorityQueue<Tile> toVisit, Func<Tile, bool> conditionToVisit)
    {

        GameMaster gameMaster = GameMaster.getInstance();
        int maximunX = gameMaster.mapWidth;
        int maximunY = gameMaster.mapHeight;
        Tile[,] map = gameMaster.mapMatrix;

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
