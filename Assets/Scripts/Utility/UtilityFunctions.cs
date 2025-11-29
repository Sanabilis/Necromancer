using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityFunctions : MonoBehaviour
{
    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void UnsetParent()
    {
        transform.SetParent(null);
    }

    public void SetAsBackgroundEffect()
    {
        UnsetParent();
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
    }
}
