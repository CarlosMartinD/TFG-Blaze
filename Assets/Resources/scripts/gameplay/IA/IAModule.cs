using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAModule : MonoBehaviour
{
    public abstract IEnumerator[] generateBehavior(Unit unit);
}
