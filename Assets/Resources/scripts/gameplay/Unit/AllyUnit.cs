using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyUnit : Unit
{
    public AllyUnit() { 
        highlightColor = Color.red;
    }
    public new bool CanMove()
    {
        SystemOperatorEngine master = FindObjectOfType<SystemOperatorEngine>();
        return !master.IsEnemyTurn() == isPlayer && !hasMoved;
    }

    protected override void Shine()
    {
        placedTile.unitPlaced = null;
        mapEngine.allyUnits.Remove(this);
    }
}
