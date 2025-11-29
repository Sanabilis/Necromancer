using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private Transform _player;
    private Rigidbody2D _rb;

    public int damage;
    public float speed;
    public float rotationSpeed;
    public float pushBackForce;

    private bool _collided = false;

    public GameObject collideParticles;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*Vector2 direction = _player.position - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        lastPosition = transform.position;*/

        Vector2 direction = _player.position - transform.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        _rb.angularVelocity = -rotationSpeed * rotateAmount;
        _rb.velocity = transform.up * speed;
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

            transform.Find("Particles").GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            transform.Find("Particles").SetParent(null);
            _collided = true;

            Instantiate(collideParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (col.tag == "Ground" && !_collided)
        {
            transform.Find("Particles").GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            transform.Find("Particles").SetParent(null);
            _collided = true;
            Instantiate(collideParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
