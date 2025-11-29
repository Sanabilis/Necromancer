using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallData
{
    public float[] position;
    public DestructibleType type;

    public WallData(GameObject wallGO)
    {
        position = new float[3];
        position[0] = wallGO.transform.position[0];
        position[1] = wallGO.transform.position[1];
        position[2] = wallGO.transform.position[2];

        if (wallGO.TryGetComponent(out DestructibleWall wall))
        {
            type = wall.type;
        }
    }
}