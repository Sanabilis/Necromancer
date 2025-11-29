using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleCollectionQuest : Quest
{
    private void Start()
    {
        QuestName = "Example Collection Quest";
        Description = "Go pick a heart";

        QuestGoals.Add(new CollectionQuestGoal(this,1,0,1, false));

        QuestGoals.ForEach(goal => goal.Init());
    }

}
