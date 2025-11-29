using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundSoulCombat : MonoBehaviour
{
    public GameObject projectile;
    public Transform startPosition;

    public void Attack()
    {
        Instantiate(projectile, startPosition.position, Quaternion.identity);
    }
}
