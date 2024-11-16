using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTile : OnTileClickedStrategy
{
    public void ExecuteStrategy(Tile tile)
    {
        GameMaster.getInstance().selectedUnit.Attack(tile.unitPlaced);
    }

    public bool IsApplicableStrategy(Tile tile)
    {
        GameMaster gameMaster = GameMaster.getInstance();
        return gameMaster.selectedUnit != null && gameMaster.selectedUnit.enemiesInRange.Count > 0 && gameMaster.selectedUnit.enemiesInRange.Contains(tile);
    }
}
