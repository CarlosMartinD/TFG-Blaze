using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTile : OnTileClickedStrategy
{
    public void ExecuteStrategy(Tile tile)
    {
        Unit attakingUnit = SystemOperatorEngine.getInstance().selectedUnit;
        attakingUnit.StartCoroutine(attakingUnit.Attack(tile.unitPlaced));
    }

    public bool IsApplicableStrategy(Tile tile)
    {
        SystemOperatorEngine gameMaster = SystemOperatorEngine.getInstance();
        return !gameMaster.isSystemBusy && gameMaster.selectedUnit != null && gameMaster.selectedUnit.enemiesInRange.Count > 0 
            && gameMaster.selectedUnit.enemiesInRange.Contains(tile);
    }
}
