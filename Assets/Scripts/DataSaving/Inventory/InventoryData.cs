using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryData
{
    public List<InventorySlotData> inventory = new List<InventorySlotData>();

    public InventoryData(GameObject playerGameObject)
    {
        InventorySO inventorySO = playerGameObject.GetComponent<Player>().GetPlayerInventory();
        if (inventorySO == null) return;
        foreach (InventorySlot inventorySlot in inventorySO.Container)
        {
            InventorySlotData inventorySlotData = new InventorySlotData(inventorySlot);
            inventory.Add(inventorySlotData);
        }
    }
}