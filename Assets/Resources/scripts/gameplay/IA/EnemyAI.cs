using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private MapEngine mapEngine;

    private TurnEngine turnEngine;

    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }
    private State state;

    void Start()
    {
        mapEngine = EngineDependencyInjector.getInstance().Resolve<MapEngine>();
        turnEngine = EngineDependencyInjector.getInstance().Resolve<TurnEngine>();
    }

    void Update()
    {
        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;

            case State.TakingTurn:
                TakeTurn();
                break;

            case State.Busy:
                break;
        }
    }

    public void OnTurnEnemyStarted()
    {
        state = State.TakingTurn;
    }

    private void TakeTurn()
    {
        state = State.Busy;
        StartCoroutine(ExecuteTurnActions());
        state = State.WaitingForEnemyTurn;
    }

    private IEnumerator ExecuteTurnActions()
    {
        bool someEnemyActed = false;
        List<EnemyUnit> selectableUnits = mapEngine.enemyUnits;

        foreach (EnemyUnit selectedUnit in selectableUnits)
        {
            IEnumerator[] behaviors = selectedUnit.iaBehavior.generateBehavior(selectedUnit);
            foreach(IEnumerator beahavior in behaviors)
            {
                someEnemyActed = true;
                yield return beahavior;
                yield return new WaitForSecondsRealtime(0.2f);
            }
        }

        if(!someEnemyActed)
        {
            yield return new WaitForSecondsRealtime(1f);
        }
        
        turnEngine.EndTurnAI();
    }

}
