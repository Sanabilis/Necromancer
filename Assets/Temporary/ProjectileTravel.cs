using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTravel : MonoBehaviour
{
    public int damage;

    void FixedUpdate()
    {
        transform.position -= new Vector3(3f * Time.deltaTime, 0f, 0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            PlayerCombat sc = col.gameObject.GetComponent<PlayerCombat>();
            sc.GetHit(damage);
            Destroy(gameObject);
        }
        else if (col.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
