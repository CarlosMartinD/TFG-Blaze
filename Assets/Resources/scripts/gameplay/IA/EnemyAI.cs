using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{   
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }
    private State state;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;

            case State.TakingTurn: 
                break;

            case State.Busy:
                break;
        }
    }

    private void OnTurnEnemyStarted()
    {
        state = State.TakingTurn;
    }

    private void TakeTurn()
    {
        state = State.Busy;

    }
}
