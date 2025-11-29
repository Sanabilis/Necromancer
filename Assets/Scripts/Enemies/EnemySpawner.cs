using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public bool shouldRespawn = true;
    private GameObject _enemy;
    public Transform initialPosition;

    void Awake()
    {
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (shouldRespawn)
        {
            GameManager.Instance.OnRespawnEnemies += Respawn;
        }

        if (enemyPrefab.tag != "Boss" || !StaticData.BossKilled)
        {
            Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Respawn()
    {
        if (!_enemy)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        _enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

        if (initialPosition != null)
        {
            _enemy.transform.Find("Soul").position = initialPosition.position;
            _enemy.transform.Find("InitialPosition").position = initialPosition.position;
        }

        if (_enemy.tag == "Boss")
        {
            _enemy.transform.parent = transform.parent;
        }
    }

    public bool IsDead()
    {
        return _enemy == null;
    }
}