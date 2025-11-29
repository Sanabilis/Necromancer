using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Item Object", menuName = "Inventory System/Items/Default")]
public class DefaultItemSO : ItemSO
{
    public void Awake()
    {
        itemType = ItemType.Default;
    }
}
