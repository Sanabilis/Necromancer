using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundSoulProjectile : MonoBehaviour
{
    public GameObject collideParticles;
    public float speed = 1f;
    public int damage = 0;
    public float pushBackForce = 1f;

    private Rigidbody2D _rb;
    private Transform _player;
    private bool _collided = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindWithTag("Player").transform;

        _rb.velocity = (_player.position - transform.position).normalized * speed;
    }

    void Update()
    {
        float angle = Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && !_collided)
        {
            PlayerCombat cSc = col.gameObject.GetComponent<PlayerCombat>();
            CharacterController mSc = col.gameObject.GetComponent<CharacterController>();

            if (!cSc.IsProtected() && !mSc.IsDashing())
            {
                cSc.GetHit(damage);
                cSc.Stun(new Vector2(Mathf.Sign(_player.position.x - transform.position.x), 2f).normalized * pushBackForce);
            }

            transform.Find("Graphics/Particles").GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            transform.Find("Graphics/Particles").SetParent(null);
            _collided = true;

            Instantiate(collideParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (col.tag == "Ground" && !_collided)
        {
            transform.Find("Graphics/Particles").GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            transform.Find("Graphics/Particles").SetParent(null);
            _collided = true;
            Instantiate(collideParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
