using System;
using System.Collections.Generic;


public class PathFinder
{
    private ISet<Tile> visited = new HashSet<Tile>();

    private MapEngine mapEngine;

    public PathFinder()
    {
       mapEngine = EngineDependencyInjector.getInstance().Resolve<MapEngine>();
    }
    
    public Stack<Tile> findShortestPath(Tile from, Tile to, ISet<Tile> limitedTiles)
    {
        var descendingComparer = Comparer<int>.Create((x, y) => y.CompareTo(x));
        PriorityQueue<Node> nodes = new PriorityQueue<Node>(descendingComparer);
        Node startingNode = new Node(from, 0, heuristchCost(from, to));
        nodes.Enqueue(startingNode, startingNode.GetF());

        while (!nodes.IsEmpty() && !nodes.Peek().currentPlace.Equals(to))
        {
            Node prioritizedNode = nodes.Dequeue();
            List<Node> neighbourds = findNeighbourds(prioritizedNode, to, limitedTiles);
            neighbourds.ForEach(neighbour => { nodes.Enqueue(neighbour, neighbour.f); });
        }

        if(!nodes.IsEmpty())
        {
            Node node = nodes.Dequeue();
            var tiles = new Stack<Tile>();
            tiles.Push(node.currentPlace);
            backtrackPathIntoStack(node, tiles);
            return tiles;
        }



        return new Stack<Tile>();
    }

    private List<Node> findNeighbourds(Node from, Tile to, ISet<Tile> limitedTiles)
    {
        List<Node> neighbours = new List<Node>();
        int[][] variation = new int[][] { new int[] { -1, 0 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { 0, 1 } };
        Tile[,] mapMatrix = mapEngine.mapMatrix;

        foreach (int[] movement in variation)
        {
            int newX = from.currentPlace.x + movement[0];
            int newY = from.currentPlace.y + movement[1];

            bool checkBoundariesX = newX >= 0  && newX < mapMatrix.GetLength(0);
            bool checkBoundariesY = newY >= 0 && newY < mapMatrix.GetLength(1);

            if (checkBoundariesX && checkBoundariesY && limitedTiles.Contains(mapMatrix[newX, newY]) && !visited.Contains(mapMatrix[newX, newY]))
            {
                Node node1 = new Node(mapMatrix[newX, newY], from.cost + 1, heuristchCost(mapMatrix[newX, newY], to), from);
                neighbours.Add(node1);
            }
        }

        visited.Add(from.currentPlace);
        return neighbours;
    }
    
    private int heuristchCost(Tile placed, Tile goal)
    {
        return Math.Abs(placed.x - goal.x) + Math.Abs(placed.y - goal.y);
    }


    private void backtrackPathIntoStack(Node start, Stack<Tile> path)
    {
        if (start.parent == null) return;

        path.Push(start.parent.currentPlace);

        backtrackPathIntoStack(start.parent, path);
    }

    private class Node
    {
        public readonly Tile currentPlace;

        public readonly int cost;

        readonly int heuristics;

        public readonly int f;

        public readonly Node parent;

        public Node(Tile currentPlace, int cost, int heuristics, Node parent)
        {
            this.currentPlace = currentPlace;
            this.cost = cost;
            this.heuristics = heuristics;
            this.f = cost + heuristics;
            this.parent = parent;
        }

        public Node(Tile currentPlace, int cost, int heuristics)
        {
            this.currentPlace = currentPlace;
            this.cost = cost;
            this.heuristics = heuristics;
            this.f = cost + heuristics;
            this.parent = null;
        }

        public int GetF()
        {
            return f;
        }

        public Tile GetCurrent ()
        {
            return currentPlace;
        }
    }
}
