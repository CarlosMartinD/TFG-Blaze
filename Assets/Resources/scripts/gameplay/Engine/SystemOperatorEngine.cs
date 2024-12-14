using System.Collections.Generic;
using UnityEngine;

public class SystemOperatorEngine : MonoBehaviour
{
    private static SystemOperatorEngine gameMaster;

    public Unit selectedUnit;
    public ISet<Unit> enemySelectedUnits;

    public bool isSystemBusy;

    public SystemOperatorEngine()
    {
        gameMaster = this;
        isSystemBusy = false;
        enemySelectedUnits = new HashSet<Unit>();    
    }

    public static SystemOperatorEngine getInstance()
    {
        return gameMaster;
    }

    void Start()
    {
        gameMaster = this;
    }

}
