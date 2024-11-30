using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public abstract class VictoryCondition : MonoBehaviour
{
    public GameObject victory;

    public GameObject defeat;

    public void Update()
    {
        checkVictoryCondition();
    }

    public void checkVictoryCondition()
    {
        if(AllyVictoryCondition())
        {
            victory.SetActive(true);
        } else if (EnemyVictoryCondition())
        {
            victory.SetActive(true);
        }
    }

    protected abstract bool AllyVictoryCondition();

    protected abstract bool EnemyVictoryCondition();
}
