using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    [SerializeField] private GameObject _stoneOverlayImageGameObject = null;
    [SerializeField] private GameObject _shieldImageGameObject = null;
    private Image _stoneOverlayImage;
    private Image _shieldImage;
    private float _shieldCooldown = 0f;
    [SerializeField] private float _deactivatedShieldAlpha = 0.6f;
    private float _shieldTimer = 0f;
    private bool _ready = true;
    private Color _initialColor;
    private float _highlightTimer;
    private bool _highlighted = false;
    [SerializeField] private float _highlightLength = 0.8f;
    [SerializeField] private float _highlightAlpha = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _stoneOverlayImage = _stoneOverlayImageGameObject.GetComponent<Image>();
        _shieldImage = _shieldImageGameObject.GetComponent<Image>();
        _initialColor = _stoneOverlayImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shieldTimer > 0f)
        {
            _shieldTimer -= Time.deltaTime;
        }
        else if (!_ready)
        {
            _ready = true;
            _shieldTimer = 0f;
            _stoneOverlayImage.color = _initialColor;
            _shieldImage.color = new Color(1f, 1f, 1f, 1f);

            Highlight();
        }

        if (_highlightTimer > 0f)
        {
            _highlightTimer -= Time.deltaTime;
        }
        else if (_highlighted)
        {
            _highlighted = false;
            _stoneOverlayImage.color = _initialColor;
        }

        SetOverlayImageColor();
    }

    public void SetShieldTimer(float cooldownLength)
    {
        _shieldCooldown = cooldownLength;
        _shieldTimer = cooldownLength;
        _ready = false;
        _shieldImage.color = new Color(1f, 1f, 1f, _deactivatedShieldAlpha);
    }

    private void Highlight()
    {
        _highlightTimer = _highlightLength;
        _highlighted = true;
    }

    private void SetOverlayImageColor()
    {
        if (!_ready)
        {
            float lengthNormalized = _shieldTimer > 0f ? _shieldTimer / _shieldCooldown : 1f;
            float a = Mathf.Lerp(0, 1, lengthNormalized);
            _stoneOverlayImage.color = new Color(_initialColor.r, _initialColor.g, _initialColor.b, a);
        }
        else if (_highlighted)
        {
            float lengthNormalized = _highlightTimer / _highlightLength;
            Color color = lengthNormalized > 0.5f
                ? Color.Lerp(new Color(1f, 1f, 1f, _highlightAlpha), new Color(1f, 1f, 1f, 0f), 2f * lengthNormalized - 1f)
                : Color.Lerp(new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 1f, _highlightAlpha), 2f * lengthNormalized);
            _stoneOverlayImage.color = color;
        }
    }
}