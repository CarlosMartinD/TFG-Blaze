using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnselectUnitTileStrategy : OnTileClickedStrategy
{

    void OnTileClickedStrategy.ExecuteStrategy(Tile tile)
    {
        SystemOperatorEngine gm = SystemOperatorEngine.getInstance();
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
        SystemOperatorEngine gm = EngineDependencyInjector.getInstance().Resolve<SystemOperatorEngine>();
        TurnEngine turnEngine = EngineDependencyInjector.getInstance().Resolve<TurnEngine>();


        return !turnEngine.IsEnemyTurn() && !gm.isSystemBusy && gm.selectedUnit == tile.unitPlaced || gm.enemySelectedUnits.Contains(tile.unitPlaced);
    }
}