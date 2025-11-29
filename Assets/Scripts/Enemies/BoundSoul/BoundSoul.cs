using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BoundSoul : EnemyScript
{
    private Vector2 _target;

    public Transform initialPosition;
    public Vector2 offset = new Vector2(3f, 3f);

    private Seeker _seeker;
    private Path _path;
    private int _currentWaypoint = 0;
    public float nextWaypointDistance = 3f;

    private float _attackTimer = 0f;
    private SpriteRenderer _renderer;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        _renderer = transform.Find("Graphics").GetComponent<SpriteRenderer>();

        _target = initialPosition.position;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        _seeker.StartPath(transform.position, _target, OnPathComplete);
    }

    public void UpdatePath()
    {
        if (_seeker.IsDone())
            _seeker.StartPath(transform.position, _target, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    void Update()
    {
        if (_attackTimer > 0f)
            _attackTimer -= Time.deltaTime;
    }

    void LateUpdate()
    {
        Color tmp = _renderer.color;
        tmp.a = Mathf.Clamp(Vector2.Distance(_player.position, transform.position) / 4f, 0.3f, 0.8f);
        _renderer.color = tmp;
    }

    void FixedUpdate()
    {
        DetectPlayer();

        if (_playerDetected)
        {
            FacePlayer();
            if (IsInAttackRange() && Vector2.Distance(_player.position, transform.position) > 1f && _attackTimer <= 0f && SeesPlayer())
            {
                AudioManager.instance.Play("SoulCast");
                _anim.SetTrigger("Attack");
                _attackTimer = 3f;
            }

            _target = _player.transform.position + new Vector3(offset.x * Mathf.Sign(transform.position.x - _player.transform.position.x), offset.y);
        }
        else
        {
            FaceDirection();
            _target = initialPosition.position;
        }


        if (_path == null || _currentWaypoint >= _path.vectorPath.Count)
            return;

        Vector2 dir = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        Vector2 force = dir * speed * Time.fixedDeltaTime;

        _rb.AddForce(force);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, speed);

        float distance = Vector2.Distance(transform.position, _path.vectorPath[_currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            _currentWaypoint++;
        }

    }

    public override bool IsInAttackRange()
    {
        return Vector2.Distance(_player.position, transform.position) < stats.attackRangeX;
    }

    public override void DetectPlayer()
    {
        _playerDetected = Vector2.Distance(_player.position, transform.position) < stats.detectRange;
    }

    public bool SeesPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (_player.position - _detectCheck.position).normalized, stats.detectRange, LayerMask.GetMask("Player", "Ground"));
        return (hit.collider != null && hit.transform.gameObject.tag == "Player");
    }
}
