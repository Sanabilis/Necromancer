using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffects : MonoBehaviour
{
    public GameObject echoEffect;

	public Color hitColor;
    public Color glowColor;
    private float _hitTimer = 0f;

    private SpriteRenderer _myRenderer;
    public SpriteRenderer glowRenderer = null;

    private Shader _shaderGUItext;
    private Shader _shaderSpritesDefault;
    private Shader _shaderSpritesUnlit;

    public GameObject jumpParticles;
    public GameObject takeOffParticles;
    public GameObject landParticles;
    public GameObject crashParticles;
    public GameObject teleportParticles;

    public Transform groundCheck;
    public Transform wallCheck;

    void Start()
    {
        _myRenderer = gameObject.GetComponent<SpriteRenderer>();
        _shaderGUItext = Shader.Find("GUI/Text Shader");
        _shaderSpritesDefault = _myRenderer.material.shader;

        if (glowRenderer != null)
        {
            _shaderSpritesUnlit = glowRenderer.material.shader;
        }
    }

    void FixedUpdate()
    {
        if (_hitTimer > 0f)
        {
            _hitTimer -= Time.fixedDeltaTime;
        }
        else if (_hitTimer > -1f)
        {
            EnemyNormal();
        }
    }

    public void EnemyHit()
    {
        _myRenderer.material.shader = _shaderGUItext;
        _myRenderer.color = hitColor;

        if (glowRenderer != null)
        {
            glowRenderer.material.shader = _shaderGUItext;
            glowRenderer.color = hitColor;
        }

        _hitTimer = 1f / 12f;
    }

    public void EnemyGlow()
    {
        _myRenderer.material.shader = _shaderGUItext;
        _myRenderer.color = glowColor;

        if (glowRenderer != null)
        {
            glowRenderer.material.shader = _shaderGUItext;
            glowRenderer.color = glowColor;
        }

        _hitTimer = 1f / 12f;
    }

    public void EnemyNormal()
    {
        _myRenderer.material.shader = _shaderSpritesDefault;
        _myRenderer.color = Color.white;

        if (glowRenderer != null)
        {
            glowRenderer.material.shader = _shaderSpritesUnlit;
            glowRenderer.color = Color.white;
        }

        _hitTimer = -2f;
    }

    public void SpawnEcho()
	{
		GameObject echo = Instantiate(echoEffect, transform.position, Quaternion.identity);
		echo.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        echo.GetComponent<SpriteRenderer>().sortingLayerName = "Enemies";
        echo.GetComponent<SpriteRenderer>().sortingOrder = -1;
        echo.transform.localScale = transform.parent.localScale;
	}

    public void LandParticles()
    {
        Vector3 offset = new Vector3(0.25f, 0f, 0f);
        Vector3 pos;
        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position - offset, -Vector3.up, 1.2f, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            pos = hit.point;
            Instantiate(landParticles, pos + offset, Quaternion.identity);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position + offset, -Vector3.up, 1.2f, LayerMask.GetMask("Ground"));

            if (hit.collider != null)
            {
                pos = hit.point;
                Instantiate(landParticles, pos - offset, Quaternion.identity);
            }
        }
    }

    public void JumpParticles()
    {
        Instantiate(takeOffParticles, groundCheck.position, Quaternion.identity, transform);
        Instantiate(jumpParticles, groundCheck.position, Quaternion.identity, transform);
    }

    public void CrashParticles()
    {
        Instantiate(crashParticles, wallCheck.position, Quaternion.identity);
    }

    public void SpawnTeleportIndicator()
    {
        Instantiate(teleportParticles, groundCheck.position, Quaternion.identity);
    }
}
