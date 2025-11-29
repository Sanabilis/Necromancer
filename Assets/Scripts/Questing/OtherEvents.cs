using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherEvents : MonoBehaviour
{
    public delegate void WaypointEventHandler(Waypoint Waypoint);
    public static event WaypointEventHandler OnWaypointReached;

    public static void WaypointReached(Waypoint Waypoint)
    {
        if (OnWaypointReached != null)
        {
            OnWaypointReached(Waypoint);
        }
    }

    public delegate void ItemEventHandler(ItemSO item);
    public static event ItemEventHandler OnItemPickedUp;

    public static void ItemPickedUp(ItemSO item)
    {
        if (OnItemPickedUp != null)
        {
            OnItemPickedUp(item);
        }
    }
}
