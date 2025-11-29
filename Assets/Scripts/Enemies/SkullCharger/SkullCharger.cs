using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullCharger : EnemyScript
{
    public float chargeSpeed = 10;
    private float _chargeTimer = 0f;
    private bool _isAttacking = false;

    private CameraScript _cam;
    private EnemyEffects _eff;

    void Start()
    {
        _cam = GameObject.FindWithTag("Cinemachine").GetComponent<CameraScript>();
        _eff = transform.Find("Graphics").GetComponent<EnemyEffects>();
    }

    void Update()
    {
        if (_anim.GetFloat("IdleTimer") >= 0f)
            _anim.SetFloat("IdleTimer", _anim.GetFloat("IdleTimer") - Time.deltaTime);

        if (_chargeTimer > 0f)
            _chargeTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        DetectPlayer();

        if (_isAttacking || !IsGrounded() || IsJumping() || IsFalling())
            _rb.drag = 0f;
        else
            _rb.drag = 25f;

        if (_anim.GetFloat("IdleTimer") >= 0f)
        {
            if (_playerDetected && Mathf.Sign(_player.position.x - transform.position.x) == transform.localScale.x &&
                _player.position.y - _rangeCheck.position.y > stats.attackRangeY)
            {
                _anim.SetFloat("IdleTimer", Mathf.Max(_anim.GetFloat("IdleTimer"), 0.5f));
            }
            
            return;
        }

        if (_anim.GetBool("IsAttacking"))
        {
            if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Charge"))
            {
                _rb.velocity = new Vector2(chargeSpeed * transform.localScale.x, _rb.velocity.y);

                if (_chargeTimer <= 0f || IsNearWall())
                {
                    if (IsNearWall())
                    {
                        AudioManager.instance.Play("Crash");
                        DestroyWall();
                        _eff.CrashParticles();
                        _cam.Shake();
                    }

                    _anim.SetBool("IsAttacking", false);
                    _anim.SetFloat("IdleTimer", 2f);
                }
            }
        }
        else if (_playerDetected && IsInAttackRange())
        {
            FacePlayer();
            _anim.SetTrigger("Attack");
            _anim.SetBool("IsAttacking", true);
            _chargeTimer = 1.5f;
        }
        else if (IsNearWall() || (IsNearEdge() && (Mathf.Sign(_player.position.x - transform.position.x) != transform.localScale.x || !_playerDetected)))
        {
            _anim.SetFloat("IdleTimer", 1f);
        }
        else
        {
            _rb.velocity = new Vector2(speed * transform.localScale.x, _rb.velocity.y);
        }
    }

    public void SetAttack()
    {
        _isAttacking = true;
    }

    public void ResetAttack()
    {
        _isAttacking = false;
    }

    void DestroyWall()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_wallCheck.position, _checkRadius * 2f, _whatIsGround);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == "Destructible")
            {
                DestructibleWall sc = colliders[i].gameObject.GetComponent<DestructibleWall>();
                sc.Destroy();
            }
        }
    }
}
