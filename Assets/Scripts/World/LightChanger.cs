using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightChanger : MonoBehaviour
{
    public Light2D globalLight;
    private bool _isActive = false;

    private Color _originalColor = Color.white;
    private float _originalIntensity = 0.4f;

    public Color color;
    public float intensity;

    private float _t = 1f;

    void Awake()
    {
        _originalColor = globalLight.color;
        _originalIntensity = globalLight.intensity;
    }

    void Start()
    {
        if (gameObject.GetComponent<PolygonCollider2D>().bounds.Contains(FindObjectOfType<Player>().transform.position))
        {
            _t = 0;
            _isActive = true;
            globalLight.color = color;
            globalLight.intensity = intensity;
        }
    }

    void Update()
    {
        _t += Time.deltaTime;
        //Debug.Log(_isActive);
        if (_isActive)
        {
            globalLight.color = Color.Lerp(_originalColor, color, Mathf.Min(_t, 1f));
            globalLight.intensity = Mathf.Lerp(_originalIntensity, intensity, Mathf.Min(_t, 1f));
        }
        else if (_t <= 1f)
        {
            globalLight.color = Color.Lerp(color, _originalColor, Mathf.Min(_t, 1f));
            globalLight.intensity = Mathf.Lerp(intensity, _originalIntensity, Mathf.Min(_t, 1f));
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            //Debug.Log("Light Changer Enter");
            _t = Mathf.Max(1f - _t, 0f);
            _isActive = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            //Debug.Log("Light Changer Exit");

            _t = Mathf.Max(1f - _t, 0f);
            _isActive = false;
        }
    }
}