using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DecimationVictoryCondtion : VictoryCondition
{

    private MapEngine mapEngine;

    void Start()
    {
        mapEngine = EngineDependencyInjector.getInstance().Resolve<MapEngine>();
    }

    protected override bool AllyVictoryCondition()
    {
        return mapEngine.enemyUnits.Count == 0;
    }

    protected override bool EnemyVictoryCondition()
    {
        return mapEngine.allyUnits.Count == 0;
    }
}
