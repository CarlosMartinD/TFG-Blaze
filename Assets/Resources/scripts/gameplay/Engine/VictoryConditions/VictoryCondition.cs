using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public abstract class VictoryCondition : MonoBehaviour
{
    public AssetBundle myLoadedAssetBundle;

    public void checkVictoryCondition()
    {
        if(AllyVictoryCondition())
        {

        } else if (EnemyVictoryCondition())
        {

        }
    }

    protected abstract bool AllyVictoryCondition();

    protected abstract bool EnemyVictoryCondition();
}
