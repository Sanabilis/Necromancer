using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleWaypointQuest : Quest
{
    private void Start()
    {
        QuestName = "Example Waypoint Quest";
        Description = "Go to Example Waypoint";

        QuestGoals.Add(new WaypointQuestGoal(this, "ExampleWaypoint"));

        QuestGoals.ForEach(goal => goal.Init());
    }

}
