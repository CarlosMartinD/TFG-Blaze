using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyUnit : Unit
{
    public AllyUnit() { 
        highlightColor = Color.red;
    }
    public bool CanMove()
    {
        GameMaster master = FindObjectOfType<GameMaster>();
        return !master.IsEnemyTurn() == isPlayer && !hasMoved;
    }
}