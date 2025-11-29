using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadCrawler : EnemyScript
{
    private float _jumpTimer = 0f;
    private BoxCollider2D _damageZone;
    private CapsuleCollider2D _groundCollider;

    void Start()
    {
        _damageZone = transform.Find("DamageZone").GetComponent<BoxCollider2D>();
        _groundCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if (_anim.GetFloat("IdleTimer") >= 0f)
            _anim.SetFloat("IdleTimer", _anim.GetFloat("IdleTimer") - Time.deltaTime);

        if (_jumpTimer > 0f)
            _jumpTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        DetectPlayer();

        if (!IsGrounded() || IsJumping() || IsFalling()) {
            _rb.drag = 0f;
            _anim.SetBool("IsGrounded", false);

            _damageZone.size = new Vector2(0.7f, 1.3f);
            _damageZone.offset = new Vector2(0f, 0f);

            _groundCollider.size = new Vector2(0.8f, 1.6f);
            _groundCollider.offset = new Vector2(0.1f, -0.2f);
        }
        else
        {
            _rb.drag = 25f;
            if (_jumpTimer <= 0f && _anim.GetCurrentAnimatorStateInfo(0).IsName("TakeOff"))
            {
                _anim.SetTrigger("Land");
            }
            _anim.SetBool("IsGrounded", true);

            _damageZone.size = new Vector2(0.7f, 1f);
            _damageZone.offset = new Vector2(0.1f, -0.5f);

            _groundCollider.size = new Vector2(0.8f, 1f);
            _groundCollider.offset = new Vector2(0.1f, -0.5f);
        }

        if (_anim.GetFloat("IdleTimer") >= 0.1f)
        {
            if (_playerDetected && !IsInAttackRange() && (IsNearEdge() || IsNearWall()) &&
                Mathf.Sign(_player.position.x - transform.position.x) == transform.localScale.x)
            {
                _anim.SetFloat("IdleTimer", Mathf.Max(_anim.GetFloat("IdleTimer"), 0.2f));
            }
            return;
        }

        if (IsGrounded() && !_anim.GetBool("IsAttacking"))
        {
            if (_playerDetected && IsInAttackRange())
            {
                FacePlayer();
                _anim.SetTrigger("Attack");
                _anim.SetBool("IsAttacking", true);
                _jumpTimer = 3f / 24f;
            }
            else if ((IsNearWall() || IsNearEdge()) && _anim.GetFloat("IdleTimer") < 0f)
            {
                _anim.SetFloat("IdleTimer", 1f);
            }
            else if (_anim.GetFloat("IdleTimer") < 0f)
            {
                _rb.velocity = new Vector2(speed * transform.localScale.x, _rb.velocity.y);
            }
        }
    }
}
