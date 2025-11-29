using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemsData
{
    public ItemData[] itemsList;

    public ItemsData(GameObject itemsGO)
    {
        int childCount = itemsGO.transform.childCount;
        itemsList = new ItemData[childCount];
        for (int i = 0; i < childCount; i++)
        {
            GameObject itemGO = itemsGO.transform.GetChild(i).gameObject;
            ItemData itemData = new ItemData(itemGO);
            itemsList[i] = itemData;
        }
    }
}