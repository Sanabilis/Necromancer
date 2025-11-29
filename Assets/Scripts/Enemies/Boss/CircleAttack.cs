using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttack : MonoBehaviour
{
    public float rotationSpeed = 90f;
    private float _Timer = 5f;

    void FixedUpdate()
    {
        if (_Timer > 0f)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.fixedDeltaTime);
            _Timer -= Time.fixedDeltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
