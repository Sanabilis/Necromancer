using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEvents : MonoBehaviour
{
    public delegate void EnemyEventHandler(EnemyScript Enemy);
    public static event EnemyEventHandler OnEnemyDeath;

    public static void EnemyDied(EnemyScript Enemy)
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(Enemy);
        }
    }
}
