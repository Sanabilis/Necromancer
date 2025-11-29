using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestGoal
{
    [NonSerialized] public Quest Quest;
    public int CurrentAmount;
    public int RequiredAmount;
    public bool Completed;

    public virtual void Init()
    {
        //nothing for now;
    }

    public void Evaluate()
    {
        if (CurrentAmount >= RequiredAmount)
        {
            Complete();
        }
    }

    public void Complete()
    {
        Completed = true;
        Quest.CheckGoals();
    }
}