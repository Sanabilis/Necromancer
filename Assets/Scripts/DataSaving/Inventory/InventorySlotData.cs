using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlotData
{
    public InventoryItemData item;
    public int amount;

    public InventorySlotData(InventorySlot inventorySlot)
    {
        item = new InventoryItemData(inventorySlot.item);
        amount = inventorySlot.amount;
    }
}