using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaypointQuestGoal : QuestGoal
{
    public string WaypointName;

    public WaypointQuestGoal(Quest quest, string waypointName)
    {
        this.Quest = quest;
        this.WaypointName = waypointName;
    }

    public override void Init()
    {
        base.Init();
        OtherEvents.OnWaypointReached += WaypointReached;
    }

    public void WaypointReached(Waypoint waypoint)
    {
        if (this.WaypointName == waypoint.waypointName)
        {
            Complete();
        }
    }
}