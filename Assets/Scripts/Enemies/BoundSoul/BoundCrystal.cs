using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BoundCrystal : EnemyScript
{
    public EnemyEffects soulEffects;

    public Light2D crystalLight;
    public Light2D soulLight;

    public BoundParticles particles;

    public Color red;
    public Color blue;

    private float _hitTimer = 0f;

    void Update()
    {
        if (_hitTimer > 0f)
        {
            _hitTimer -= Time.deltaTime;
        }
        else if (_hitTimer > -1f)
        {
            crystalLight.color = red;
            soulLight.color = red;
            _hitTimer = -2f;
        }
    }

    public override void GetHit(int damage)
    {
        soulEffects.EnemyHit();
        crystalLight.color = blue;
        soulLight.color = blue;
        _hitTimer = 1f / 12f;
        particles.OnHit();
        base.GetHit(damage);
    }

    void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
