using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int life;

    public int attack;

    public int deffense;

    public int velocity;

    public bool LifePointsVariation(int lifeVariation)
    {
        life -= lifeVariation;
        return lifeVariation > 0;
    }





}
