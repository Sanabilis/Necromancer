using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    public string waypointName;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Waypoint reached " + waypointName);
        OtherEvents.WaypointReached(this);
    }

}

