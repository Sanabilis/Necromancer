using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCamera : MonoBehaviour
{
    public Animator _anim;

    public EnemySpawner boss;
    private bool wallDestroyed = false;
    private GameObject _wall;
    private float _volumeMultiplier = 1f;
    private float _volume;

    public GameObject toDestroy;

    void Awake()
    {
        if (StaticData.BossKilled)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _wall = transform.Find("SolidWall").gameObject;
        _volume = AudioManager.instance.music.source.volume;
    }

    void Update()
    {
        if (boss.IsDead() && AudioManager.instance.music.source.isPlaying && _volumeMultiplier > 0.01f)
        {
            _volumeMultiplier -= Time.deltaTime / 4f;
            AudioManager.instance.music.source.volume = _volume * _volumeMultiplier;
            AudioManager.instance.music.volume = _volume * _volumeMultiplier;
        }
        else if (_volumeMultiplier <= 0.01f && AudioManager.instance.music.source.isPlaying)
        {
            AudioManager.instance.music.source.Stop();
        }

        if (boss.IsDead())
        {
            _anim.SetBool("Focus", false);
        }

        if (!wallDestroyed && boss.IsDead())
        {
            _wall.GetComponent<DestructibleWall>().Destroy();
            Destroy(toDestroy);
            wallDestroyed = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && !boss.IsDead())
        {
            _anim.SetBool("Focus", true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            _anim.SetBool("Focus", false);
        }
    }
}
