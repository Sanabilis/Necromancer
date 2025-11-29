using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyStats : ScriptableObject
{
    public int maxHealth;

    public int damage;

    public float detectRange;

    public float attackRangeX;
    public float attackRangeY;
}
