using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    public PopUpMessage popup = null;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (popup) popup.Display();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Checkpoint", true);
    }
}