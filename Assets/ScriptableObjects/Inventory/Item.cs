using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO item;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (item)
            {
                // Debug.Log(item);
                other.GetComponent<Player>().AddItem(item, 1);
                OtherEvents.ItemPickedUp(this.item);
                Destroy(gameObject);
            }
        }
    }

}
