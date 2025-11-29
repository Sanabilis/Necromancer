using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttackManager : MonoBehaviour
{
    public GameObject circleAttack1;
    public GameObject circleAttack2;
    public float pauseAttackTimer = 4f;

    void Start()
    {
        Instantiate(circleAttack1, transform.position, Quaternion.identity);
    }

    void Update()
    {
        if (pauseAttackTimer > 0f)
        {
            pauseAttackTimer -= Time.deltaTime;
        }
        else
        {
            Instantiate(circleAttack2, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
