using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadCrawlerCombat : MonoBehaviour
{
    private Transform _player;
    private Rigidbody2D _rb;
    public float jumpTime = 1.25f;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform.Find("GroundCheck");
        _rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
        _rb.drag = 0f;

        float velX = (_player.position.x - transform.position.x) / jumpTime;
        float velY = (_player.position.y - transform.position.y - 0.5f * Physics2D.gravity.y * _rb.gravityScale * jumpTime * jumpTime) / jumpTime;

        _rb.velocity = new Vector2(velX, velY);
    }
}
