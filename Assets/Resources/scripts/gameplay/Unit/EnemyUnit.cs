using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    public EnemyUnit()
    {
        this.isPlayer = false;
        this.highlightColor = Color.gray;
    }
}