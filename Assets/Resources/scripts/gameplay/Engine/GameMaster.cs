using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster gameMaster;

    public MapEngine mapEngine;

    public Unit selectedUnit;
    public ISet<Unit> enemySelectedUnits;
    public GameObject mapParent;

    private bool enemyTurn = false;

    public bool somethingTakingAction;

    public GameMaster()
    {
        gameMaster = this;
        somethingTakingAction = false;
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }
    }

   private void EndTurn()
    {
        enemyTurn = !enemyTurn;


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

    public bool IsEnemyTurn()
    {
        return enemyTurn;
    }
}
