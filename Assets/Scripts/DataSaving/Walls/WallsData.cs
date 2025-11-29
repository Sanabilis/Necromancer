using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallsData
{
    public WallData[] wallsList;

    public WallsData(GameObject wallsGO)
    {
        int childCount = wallsGO.transform.childCount;
        wallsList = new WallData[childCount];
        for (int i = 0; i < childCount; i++)
        {
            GameObject wallGO = wallsGO.transform.GetChild(i).gameObject;
            WallData wallData = new WallData(wallGO);
            wallsList[i] = wallData;
        }
    }
}