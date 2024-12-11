using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{

    public IAModule iaBehavior;


    public EnemyUnit()
    {
        this.isPlayer = false;
        this.highlightColor = Color.gray;
    }

    protected override void Shine()
    {
        placedTile.unitPlaced = null;
        mapEngine.enemyUnits.Remove(this);
    }
}
