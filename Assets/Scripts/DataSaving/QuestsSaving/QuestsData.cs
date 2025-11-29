using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestsData
{
    public List<QuestData> quests;

    public QuestsData(GameObject questsGameObject)
    {
        Quest[] qs = questsGameObject.GetComponents<Quest>();
        quests = new List<QuestData>(qs.Length);
        foreach (Quest q in qs)
        {
            QuestData questData = new QuestData(q);
            quests.Add(questData);
        }
    }
}