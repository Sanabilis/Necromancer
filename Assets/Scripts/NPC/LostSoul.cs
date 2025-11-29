using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostSoul : MonoBehaviour
{
    public ItemSO item;
    private Transform _target;
    private Rigidbody2D _rb;
    public bool follow = false;

    void Start()
    {
        _target = GameObject.FindWithTag("Player").transform.Find("FollowPosition");
        _rb = GetComponent<Rigidbody2D>();
        if (_target.parent.gameObject.GetComponent<Player>().GetPlayerInventory().HasItem(item) > 0)
        {
            follow = true;
            transform.position = _target.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (follow)
        {
            _rb.velocity = (_target.position - transform.position) * 5f;
        }
    }

    public void GiveItem()
    {
        if (item)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<Player>().AddItem(item, 1);
            OtherEvents.ItemPickedUp(this.item);
        }
    }
}
