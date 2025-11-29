using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example2KillQuest : Quest
{
    private void Start()
    {
        QuestName = "Example Kill Quest";
        Description = "Kill 2 Skull Crusher";

        QuestGoals.Add(new KillQuestGoal(this,EnemyType.SkullCharger, 0, 2,false));

        QuestGoals.ForEach(goal => goal.Init());
}


}
