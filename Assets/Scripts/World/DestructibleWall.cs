using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DestructibleType { Fragile, Destructible, Solid };

public class DestructibleWall : MonoBehaviour
{
    public DestructibleType type = DestructibleType.Destructible;
    public CameraScript _camScript;

    private bool _destroyed = false;

    void Start()
    {
        _camScript = GameObject.FindWithTag("Cinemachine").GetComponent<CameraScript>();
    }

    public void Destroy()
    {
        if (!_destroyed)
        {
            AudioManager.instance.Play("RockRumble");
            Destroy(gameObject);
            _camScript.Shake();
            _destroyed = true;
        }
    }
}
