using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnselectUnitTileStrategy : OnTileClickedStrategy
{

    void OnTileClickedStrategy.ExecuteStrategy(Tile tile)
    {
        GameMaster gm = GameMaster.getInstance();
        if (gm.selectedUnit == tile.unitPlaced)
        {
            gm.selectedUnit = null;
        } else
        {
            gm.enemySelectedUnits.Remove(tile.unitPlaced);
        }

        tile.unitPlaced.RemoveMovementCandidates();

    }

    bool OnTileClickedStrategy.IsApplicableStrategy(Tile tile)
    {
        if(!tile.unitPlaced)
        {
            return false;
        }
        GameMaster gm = GameMaster.getInstance();

        return !gm.isSystemBusy && gm.selectedUnit == tile.unitPlaced || gm.enemySelectedUnits.Contains(tile.unitPlaced);
    }
}