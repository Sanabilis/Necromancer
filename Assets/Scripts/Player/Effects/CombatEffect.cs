using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEffect : MonoBehaviour
{
    public GameObject healParticles;

    public Color glowColor;
    public Color hitColor;

    private SpriteRenderer _myRenderer;
    private Shader _shaderGUItext;
    private Shader _shaderSpritesDefault;

    private float _healTimer = 0f;
    private bool _normalColor = true;

    void Start()
    {
        _myRenderer = gameObject.GetComponent<SpriteRenderer>();
        _shaderGUItext = Shader.Find("GUI/Text Shader");
        _shaderSpritesDefault = _myRenderer.material.shader;
    }

    void Update()
    {
        if (_healTimer > 0f)
        {
            _healTimer -= Time.deltaTime;
        }
        else if (_healTimer <= 0f && !_normalColor)
        {
            PlayerNormal();
        }
    }

    public void PlayerGlow()
    {
        _myRenderer.material.shader = _shaderGUItext;
        _myRenderer.color = glowColor;
        _healTimer = 2f / 12f;
        _normalColor = false;
    }

    public void PlayerHit()
    {
        _myRenderer.material.shader = _shaderGUItext;
        _myRenderer.color = hitColor;
    }

    public void PlayerNormal()
    {
        _myRenderer.material.shader = _shaderSpritesDefault;
        _myRenderer.color = Color.white;
        _normalColor = true;
    }

    public void HealParticles()
    {
        GameObject healP = Instantiate(healParticles, transform.position, Quaternion.identity);
        ParticleSystem ps = healP.GetComponent<ParticleSystem>();
        var col = ps.colorOverLifetime;

        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(glowColor, 0.75f), new GradientColorKey(glowColor, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.75f), new GradientAlphaKey(0.0f, 1.0f) });

        col.color = grad;
    }
}
