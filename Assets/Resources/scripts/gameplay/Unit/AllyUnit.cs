using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyUnit : Unit
{
    private TurnEngine turnEngine;

    public AllyUnit() { 
        highlightColor = Color.red;
    }


    public new bool CanMove()
    {
        if(turnEngine == null)
        {
            turnEngine = EngineDependencyInjector.getInstance().Resolve<TurnEngine>();
        }
        return !turnEngine.IsEnemyTurn() == isPlayer && !hasMoved;
    }

    protected override void Shine()
    {
        placedTile.unitPlaced = null;
        mapEngine.allyUnits.Remove(this);
    }
}
