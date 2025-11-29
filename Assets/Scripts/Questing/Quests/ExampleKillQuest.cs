using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleKillQuest : Quest
{
    private void Start()
    {
        QuestName = "Example Kill Quest";
        Description = "Kill Skull Crusher";

        QuestGoals.Add(new KillQuestGoal(this,EnemyType.SkullCharger, 0, 1,false));

        QuestGoals.ForEach(goal => goal.Init());
}


}
