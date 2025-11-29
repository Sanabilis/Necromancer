using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventorySO : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();

    public void AddItem(ItemSO item, int amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item.id == item.id)
            {
                Container[i].AddAmount(amount);
                hasItem = true;
                break;
            }
        }

        if (!hasItem)
        {
            Container.Add(new InventorySlot(item, amount));
        }
    }

    public int HasItem(ItemSO item)
    {
        InventorySlot foundItem = Container.Find(x => x.item.id == item.id);
        return foundItem?.amount ?? 0;
    }

    public void RemoveItem(ItemSO item, int amount)
    {
        bool removeItem = false;
        int removeAt = -1;

        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item.id == item.id)
            {
                if (Container[i].amount > amount)
                {
                    Container[i].RemoveAmount(amount);
                }
                else if (Container[i].amount == amount)
                {
                    removeItem = true;
                    removeAt = i;
                }

                break;
            }
        }

        if (removeItem)
        {
            Container.RemoveAt(removeAt);
        }
    }
}