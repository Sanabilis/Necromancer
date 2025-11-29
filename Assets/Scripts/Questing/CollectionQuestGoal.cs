using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CollectionQuestGoal : QuestGoal
{
    public int ItemId;

    public CollectionQuestGoal(Quest quest, int itemId, int currentAmount, int requiredAmount, bool completed)
    {
        this.Quest = quest;
        this.ItemId = itemId;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
        this.Completed = completed;
    }

    public override void Init()
    {
        base.Init();
        OtherEvents.OnItemPickedUp += ItemPickedUp;
    }

    public void ItemPickedUp(ItemSO item)
    {
        if (item.id == this.ItemId)
        {
            this.CurrentAmount++;
            Evaluate();
        }
    }
}