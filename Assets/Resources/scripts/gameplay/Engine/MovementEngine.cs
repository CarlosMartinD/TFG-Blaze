using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class MovementEngine
{

    public HashSet<Tile> highlitedTiles = new HashSet<Tile>();
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

    public void GetWalkableTiles(Tile tile, Unit unit)
    {
        GameMaster gameMaster = GameMaster.getInstance();
        int maximunX = gameMaster.mapWidth;
        int maximunY = gameMaster.mapHeight;

        HashSet<Tile> visitedSquares = new HashSet<Tile>
        {
            tile
        };
        if (tile.x + 1 < maximunX) { HighLightAdyenctTilesToHighLight(tile.x + 1, tile.y, unit.movementCapacity, visitedSquares); }
        if (tile.x - 1 >= 0) { HighLightAdyenctTilesToHighLight(tile.x - 1, tile.y, unit.movementCapacity, visitedSquares); }
        if (tile.y + 1 < maximunY) { HighLightAdyenctTilesToHighLight(tile.x, tile.y + 1, unit.movementCapacity, visitedSquares); }
        if (tile.y - 1 >= 0) { HighLightAdyenctTilesToHighLight(tile.x, tile.y - 1, unit.movementCapacity, visitedSquares); }
    }

    private void HighLightAdyenctTilesToHighLight(int x, int y, int movementPosible, HashSet<Tile> visitedSquares)
    {
        GameMaster gameMaster = GameMaster.getInstance();
        Tile tile = gameMaster.mapMatrix[x, y];
        if (movementPosible == 0 || visitedSquares.Contains(tile))
        {
            return;
        }

        int maximunX = gameMaster.mapWidth;
        int maximunY = gameMaster.mapHeight;

        int reducedMovement = movementPosible - 1;
        if (x + 1 < maximunX) { HighLightAdyenctTilesToHighLight(x + 1, y, reducedMovement, visitedSquares); }
        if (x - 1 >= 0) { HighLightAdyenctTilesToHighLight(x - 1, y, reducedMovement, visitedSquares); }
        if (y + 1 < maximunY) { HighLightAdyenctTilesToHighLight(x, y + 1, reducedMovement, visitedSquares); }
        if (y - 1 >= 0) { HighLightAdyenctTilesToHighLight(x, y - 1, reducedMovement, visitedSquares); }

        tile.Highlight();
        highlitedTiles.Add(tile);
        visitedSquares.Add(tile);
    }

    public void ResetMap()
    {
        foreach (Tile tile in highlitedTiles)
        {
            tile.Reset();
        }

        highlitedTiles.Clear();
        GameMaster.getInstance().selectedUnit = null;
    }

    
}
