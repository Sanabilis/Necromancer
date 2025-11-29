using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    public GameObject projectile;
    private float _spawnTimer = 3f;

    void Update()
    {
        if (_spawnTimer > 0f)
        {
            _spawnTimer -= Time.deltaTime;
        }
        else
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            _spawnTimer = 3f;
        }
    }
}
