using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster gameMaster;

    public MapEngine mapEngine;

    public Unit selectedUnit;
    public ISet<Unit> enemySelectedUnits;

    private bool enemyTurn = false;

    public bool isSystemBusy;

    public GameMaster()
    {
        gameMaster = this;
        isSystemBusy = false;
        enemySelectedUnits = new HashSet<Unit>();    
    }

    public static GameMaster getInstance()
    {
        return gameMaster;
    }

    void Start()
    {
        gameMaster = this;
    }

   public void EndTurn()
    {
        enemyTurn = !enemyTurn;
        cleanTurn();
         
        if (selectedUnit != null)
        {
            selectedUnit = null;
        }

        if (enemyTurn)
        {
            EnemyAI enemyAI = FindObjectOfType<EnemyAI>();
            enemyAI.OnTurnEnemyStarted();
        }
    }

    private void cleanTurn()
    {
        foreach(Unit unit in mapEngine.allyUnits)
        {
            unit.ResetUnit();
        }

        foreach (Unit unit in mapEngine.enemyUnits)
        {
            unit.ResetUnit();
        }
    }

    public bool IsEnemyTurn()
    {
        return enemyTurn;
    }
}
