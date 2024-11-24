using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStrategyOnTileClick : OnTileClickedStrategy
{
    void OnTileClickedStrategy.ExecuteStrategy(Tile tile)
    {
        Unit unitToMove = GameMaster.getInstance().selectedUnit;
        unitToMove.StartCoroutine(unitToMove.Move(tile));
    }

    bool OnTileClickedStrategy.IsApplicableStrategy(Tile tile)
    {
        GameMaster gameMaster = GameMaster.getInstance();
        return gameMaster.selectedUnit != null && gameMaster.selectedUnit.CanMove();
    }
}
