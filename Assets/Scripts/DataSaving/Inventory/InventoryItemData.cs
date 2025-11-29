using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public int id;

    public InventoryItemData(ItemSO itemSO)
    {
        id = itemSO.id;
    }
}