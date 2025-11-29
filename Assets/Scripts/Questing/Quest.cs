using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest : MonoBehaviour
{
    public List<QuestGoal> QuestGoals { get; set; } = new List<QuestGoal>();

    public string QuestName;
    public string Description;
    public bool Completed;
    public QuestGiver Npc;

    public void CheckGoals()
    {
        Completed = QuestGoals.All(goal => goal.Completed);
    }

    private void Start()
    {
        if (Npc != null)
        {
            Npc.HasAssignedQuest = !Completed;
            Npc.WasQuestComleted = Completed;
        }
    }

    //TODO: Decide what rewards we gonna be giving out

    public void LoadQuest(QuestData questData, QuestGiver questGiver)
    {
        Debug.Log("LoadQuest");
        Completed = questData.completed;
        Npc = questGiver;
        QuestGoals = questData.questGoals;
        foreach (QuestGoal questGoal in QuestGoals)
        {
            questGoal.Quest = this;
        }
    }
}