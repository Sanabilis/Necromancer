using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    public Boss boss;
    public List<Transform> _positions = new List<Transform>();
    private int _posIndex = 0;

    public GameObject circleAttack;
    public GameObject castAttack;
    public Transform projectileSpawn;
    private Transform _toDestroy;

    private Animator _anim;
    public GameObject crawler;
    private GameObject _player;
    public GameObject damageZone;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player");
    }

    public void SpawnCircleAttack()
    {
        if (_toDestroy == null)
        {
            _toDestroy = transform.parent.parent.Find("ToDestroy");
        }

        GameObject attack = Instantiate(circleAttack, transform.position, Quaternion.identity);
        attack.transform.parent = _toDestroy;
    }

    public void SpawnCastAttack()
    {
        if (_toDestroy == null)
        {
            _toDestroy = transform.parent.parent.Find("ToDestroy");
        }

        GameObject attack = Instantiate(castAttack, projectileSpawn.position, Quaternion.Euler(0f, 0f, transform.parent.localScale.x * -90f));
        attack.transform.parent = _toDestroy;
    }

    public void SpawnCrawlers()
    {
        if (_toDestroy == null)
        {
            _toDestroy = transform.parent.parent.Find("ToDestroy");
        }

        GameObject enemy;
        if (transform.position.x > _player.transform.position.x)
        {
            enemy = Instantiate(crawler, transform.position + new Vector3(3f, 0f, 0f), Quaternion.identity);
        }
        else
        {
            enemy = Instantiate(crawler, transform.position - new Vector3(3f, 0f, 0f), Quaternion.identity);
        }

        enemy.transform.Find("Graphics").gameObject.GetComponent<Animator>().Play("Leap");
        enemy.transform.parent = _toDestroy;
    }

    public void Teleport()
    {
        if (_positions.Count < 3)
        {
            _positions.Add(transform.parent.parent.Find("Pos1"));
            _positions.Add(transform.parent.parent.Find("Pos2"));
            _positions.Add(transform.parent.parent.Find("Pos3"));
        }

        int next = Random.Range(0, _positions.Count);

        while (next == _posIndex)
        {
            next = Random.Range(0, _positions.Count);
        }

        _posIndex = next;
        transform.parent.position = _positions[_posIndex].position;

    }

    public void SetIdleTimer( float t )
    {
        boss.SetTimer(t);
    }

    public void SetTeleport()
    {
        boss.SetTeleport();
    }

    public void DecreaseCastCount()
    {
        _anim.SetInteger("CastCount", _anim.GetInteger("CastCount") - 1);
    }

    public void SetDamageZone()
    {
        damageZone.SetActive(!damageZone.activeSelf);
    }

    void CastSound()
    {
        AudioManager.instance.Play("BossCast" + (_anim.GetInteger("CastCount") % 2).ToString());
    }

    void CircleSound()
    {
        AudioManager.instance.Play("BossCircle");
    }

    void TeleportSound()
    {
        AudioManager.instance.Play("BossTeleport2");
    }
}
