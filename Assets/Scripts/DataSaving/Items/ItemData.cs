using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public float[] position;

    public ItemData(GameObject itemGO)
    {
        position = new float[3];
        position[0] = itemGO.transform.position[0];
        position[1] = itemGO.transform.position[1];
        position[2] = itemGO.transform.position[2];
    }
}