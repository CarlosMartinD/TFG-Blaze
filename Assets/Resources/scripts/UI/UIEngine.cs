using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEngine : MonoBehaviour
{
    public DamageIcon damageIcon;

    public void showDamage(Vector3 position, int damage)
    {
        DamageIcon instance = Instantiate(damageIcon, position, Quaternion.identity);
    }

}
