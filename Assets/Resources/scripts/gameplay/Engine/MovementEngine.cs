using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class MovementEngine
{

    private static MovementEngine movementEngine;

    private MovementEngine()
    {

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

        HashSet<Tile> visitedSquares = new HashSet<Tile>
        {
            from
        };


        HashSet<Tile> setToFill = new HashSet<Tile>();
        MoveIntoNeighbourds(from, range, visitedSquares, setToFill);
        return setToFill;
    }

    private void VisitTileUnderCondition(Tile from, int movementPosible, ISet<Tile> visitedSquares, ISet<Tile> setToFill)
    {
        if (movementPosible < 0 || visitedSquares.Contains(from))
        {
            return;
        }

        MoveIntoNeighbourds(from, movementPosible, visitedSquares, setToFill);
        setToFill.Add(from);
        visitedSquares.Add(from);
    }

    private void MoveIntoNeighbourds(Tile from, int movementPossible, ISet<Tile> visitedSquares, ISet<Tile> setToFill)
    {
        GameMaster gameMaster = GameMaster.getInstance();
        int maximunX = gameMaster.mapWidth;
        int maximunY = gameMaster.mapHeight;
        Tile [,] map = gameMaster.mapMatrix;
        
        int[][] variation = new int[][] { new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 } };

        foreach (int[] movement in variation)
        {
            int newX = from.x + movement[0];
            int newY = from.y + movement[1];

            bool checkBoundariesX = newX >= 0 && newX < maximunX;
            bool checkBoundariesY = newY >= 0 && newY < maximunY;

            if (checkBoundariesX && checkBoundariesY)
            {
                VisitTileUnderCondition(map[newX, newY], movementPossible - 1, visitedSquares, setToFill);
            }
        }
    }
}
