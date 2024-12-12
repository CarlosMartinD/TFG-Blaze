using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStrategyOnTileClick : OnTileClickedStrategy
{
    void OnTileClickedStrategy.ExecuteStrategy(Tile tile)
    {
        Unit unitToMove = SystemOperatorEngine.getInstance().selectedUnit;
        unitToMove.StartCoroutine(unitToMove.Move(tile));
    }

    bool OnTileClickedStrategy.IsApplicableStrategy(Tile tile)
    {
        SystemOperatorEngine gameMaster = SystemOperatorEngine.getInstance();
        return !gameMaster.isSystemBusy && gameMaster.selectedUnit != null && gameMaster.selectedUnit.CanMove();
    }
}
