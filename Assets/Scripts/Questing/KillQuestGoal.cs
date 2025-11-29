using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KillQuestGoal : QuestGoal
{
    public EnemyType EnemyType;

    public KillQuestGoal(Quest quest, EnemyType enemyType, int currentAmount, int requiredAmount, bool completed)
    {
        this.Quest = quest;
        this.EnemyType = enemyType;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
        this.Completed = completed;
    }

    public override void Init()
    {
        base.Init();
        CombatEvents.OnEnemyDeath += EnemyDied;
    }

    public void EnemyDied(EnemyScript enemy)
    {
        if (enemy.type == this.EnemyType)
        {
            this.CurrentAmount++;
            Evaluate();
        }
    }
}