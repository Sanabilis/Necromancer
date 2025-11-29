using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    public bool shouldListenForInput;

    public virtual void Interact()
    {
        Debug.Log("Interacting with base interactable method Interact");
    }

    public virtual void DialogFinished()
    {
        Debug.Log("Interacting with base interactable method DialogFinished");
    }
}
