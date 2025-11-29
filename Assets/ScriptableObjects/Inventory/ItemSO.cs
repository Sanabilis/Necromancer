using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default,
    LostSoul
}

public class ItemSO : ScriptableObject
{
    public GameObject prefab;
    public ItemType itemType;
    public int id;

    [TextArea(15, 20)]
    public string description;

}
