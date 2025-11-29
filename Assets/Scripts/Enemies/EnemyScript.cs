using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    SkullCharger,
    UndeadCrawler,
    BoundSoul,
    Boss
};

public class EnemyScript : MonoBehaviour
{
    public EnemyStats stats;
    public EnemyType type;
    protected int _health;

    public float speed = 2;

    public float pushBackForce = 10f;

    protected Rigidbody2D _rb;
    protected Animator _anim;

    protected Transform _player;
    protected bool _playerDetected = false;

    [SerializeField] protected Transform _groundCheck = null;
    [SerializeField] protected Transform _wallCheck = null;
    [SerializeField] protected Transform _fallCheck = null;
    [SerializeField] protected Transform _rangeCheck = null;
    [SerializeField] protected Transform _detectCheck = null;

    [SerializeField] protected LayerMask _whatIsGround = 0;

    public GameObject deathAnimation;

    protected const float _checkRadius = .2f;

    void Awake()
    {
        GameManager.Instance.OnPause += PauseAudio;

        _health = stats.maxHealth;

        _rb = GetComponent<Rigidbody2D>();
        _anim = transform.Find("Graphics").GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    public void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
    }

    public void FacePlayer()
    {
        if (_playerDetected)
        {
            transform.localScale = new Vector3(Mathf.Sign(_player.position.x - transform.position.x), 1f, 1f);
        }
    }

    public void FaceDirection()
    {
        transform.localScale = new Vector3(Mathf.Sign(_rb.velocity.x), 1f, 1f);
    }

    public virtual void DetectPlayer()
    {
        //Debug.DrawRay(transform.position, (_player.position - _detectCheck.position).normalized, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (_player.position - _detectCheck.position).normalized,
            stats.detectRange, LayerMask.GetMask("Player", "Ground"));
        _playerDetected = (hit.collider != null && hit.transform.gameObject.tag == "Player");
    }

    public virtual bool IsInAttackRange()
    {
        return (Mathf.Abs(_rangeCheck.position.x - _player.position.x) < stats.attackRangeX &&
                Mathf.Abs(_rangeCheck.position.y - _player.position.y) < stats.attackRangeY);
    }

    public virtual bool IsGrounded()
    {
        return (Physics2D.OverlapCircleAll(_groundCheck.position, _checkRadius, _whatIsGround).Length > 0);
    }

    public virtual bool IsNearWall()
    {
        return (Physics2D.OverlapCircleAll(_wallCheck.position, _checkRadius * 2f, _whatIsGround).Length > 0);
    }

    public virtual bool IsNearEdge()
    {
        return (Physics2D.OverlapCircleAll(_fallCheck.position, _checkRadius, _whatIsGround).Length == 0);
    }

    public virtual bool IsJumping()
    {
        return _rb.velocity.y > 0.01f;
    }

    public virtual bool IsFalling()
    {
        return _rb.velocity.y < -0.01f;
    }

    public virtual void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            PlayerCombat cSc = col.gameObject.GetComponent<PlayerCombat>();
            CharacterController mSc = col.gameObject.GetComponent<CharacterController>();

            if (!cSc.IsProtected() && !mSc.IsDashing())
            {
                cSc.GetHit(stats.damage);
                cSc.Stun(new Vector2(Mathf.Sign(_player.position.x - transform.position.x), 0.75f).normalized *
                         pushBackForce);
            }
        }
    }

    public virtual void GetHit(int damage)
    {
        _health -= damage;
        AudioManager.instance.Play("SwordHitEnemy");
        if (_health <= 0)
        {
            CombatEvents.EnemyDied(this);
            Die();
        }
    }

    public virtual void Die()
    {
        GameManager.Instance.OnPause -= PauseAudio;
        Instantiate(deathAnimation, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void PauseAudio(bool paused)
    {
        if (TryGetComponent(out AudioSource audioSource))
        {
            if (paused) audioSource.Pause();
            else audioSource.UnPause();
        }
    }
}