using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleWaypointKillQuest : Quest
{
    private void Start()
    {
        QuestName = "Example Waypoint Kill Quest";
        Description = "Go to Example Waypoint and Kill Skull Crusher";

        QuestGoals.Add(new WaypointQuestGoal(this, "ExampleWaypoint"));
        QuestGoals.Add(new KillQuestGoal(this, EnemyType.SkullCharger, 0, 1, false));


        QuestGoals.ForEach(goal => goal.Init());
    }

}
