using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy" || col.tag == "Boss")
        {
            EnemyEffects effects = col.transform.Find("Graphics").GetComponent<EnemyEffects>();
            effects.EnemyHit();

            EnemyScript combat = col.gameObject.GetComponent<EnemyScript>();
            combat.GetHit(10);
        }
        else if (col.tag == "Destructible")
        {
            AudioManager.instance.Play("SwordHitRock");
            DestructibleWall sc = col.gameObject.GetComponent<DestructibleWall>();
            if (sc.type == DestructibleType.Destructible)
            {
                sc.Destroy();
            }
        }
        else if (col.tag == "Ground")
        {
            AudioManager.instance.Play("SwordHitRock");
        }
    }
}
