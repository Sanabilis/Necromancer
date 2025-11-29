using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyScript
{
    private float _idleTimer = 0f;
    private bool _teleport = false;

    void Start()
    {
        _playerDetected = true;
    }

    void Update()
    {
        bool isFar = Mathf.Abs(_player.position.x - transform.position.x) >= 3f;

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Dormant") || _anim.GetCurrentAnimatorStateInfo(0).IsName("Start"))
        {
            return;
        }

        if (_idleTimer > 0f)
        {
            _idleTimer -= Time.deltaTime;
            if (_idleTimer < 1.5f && _teleport)
            {
                AudioManager.instance.Play("BossTeleport1");
            }
        }
        else if (_teleport && _anim.GetInteger("CastCount") <= 0 && _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _anim.SetTrigger("Teleport");
            _teleport = false;
        }
        else if (!isFar && _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            float decide = Random.Range(0f, 1f);

            if (decide < 0.4f)
            {
                _anim.SetTrigger("Ressurect");
                FacePlayer();
            }
            else
            {
                _anim.SetTrigger("Circle");
            }
        }
        else if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            int count = Random.Range(2, 4);
            _anim.SetInteger("CastCount", count);

            float decide = Random.Range(0f, 1f);
            _teleport = decide < 0.5f;
            FacePlayer();
        }
    }

    public void SetTimer( float t )
    {
        _idleTimer = t;
    }

    public void SetTeleport()
    {
        _teleport = true;
    }

    public override void GetHit(int damage)
    {
        if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
        {
            _health -= damage;
            AudioManager.instance.Play("SwordHitEnemy");
            if (_health <= 0)
            {
                Instantiate(deathAnimation, transform.position, Quaternion.identity);
                CombatEvents.EnemyDied(this);
                Destroy(gameObject);
            }
        }
    }
}
