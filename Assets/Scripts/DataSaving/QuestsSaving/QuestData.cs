using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    public bool completed;
    public string npcName;
    public List<QuestGoal> questGoals;

    public QuestData(Quest quest)
    {
        completed = quest.Completed;
        npcName = quest.Npc.npcName;
        questGoals = quest.QuestGoals;
    }
}