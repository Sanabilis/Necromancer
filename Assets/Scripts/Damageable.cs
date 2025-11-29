using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{
    [SerializeField] private GameObject
        _healthBar; // GameObject with a Slider component that has Fill Rect set to an image representing amount of health

    public int maxHealth = 100;
    [Range(0.0f, 1.0f)] public float halfHealthColorPosition = 0.6f;
    public Color fullHealthColor = new Color(0f, 0.504f, 0.821f, 1f);
    public Color halfHealthColor = new Color(0.219f, 0f, 1f, 1f);
    public Color zeroHealthColor = new Color(0.651f, 0.028f, 0.176f, 1f);
    private int _health;
    private float _healthDisplayed;
    private float _healthOld;
    private float _widthOld;
    private float _maxHealthDisplayed;
    private float _maxHealthOld;
    private Slider _slider;
    public event Action OnHealthDepletion;
    private float _timerHealth;
    private float _timerSize;
    public float changeHealthAnimLength = 0.5f;
    public float changeMaxHealthAnimLength = 1f;

    void Awake()
    {
        _slider = _healthBar.GetComponent<Slider>();
        _health = maxHealth;
        _healthOld = _health;
        _healthDisplayed = _health;
        _widthOld = _healthBar.GetComponent<RectTransform>().sizeDelta.x;
        _maxHealthOld = maxHealth;
        _maxHealthDisplayed = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMaxHealthAmount(maxHealth, false);
        SetHealthAmount(_health, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerHealth >= 0f)
        {
            _timerHealth -= Time.deltaTime;
            _healthDisplayed = Mathf.Lerp(_health, _healthOld, _timerHealth / changeHealthAnimLength);
            SetHealthBarColorAndValue();
        }
        else if (Mathf.Abs(_health - _healthOld) > 0.001f)
        {
            _healthOld = _health;
            _healthDisplayed = _health;
        }

        if (_timerSize >= 0f)
        {
            _timerSize -= Time.deltaTime;
            _maxHealthDisplayed = Mathf.Lerp(maxHealth, _maxHealthOld, _timerSize / changeMaxHealthAnimLength);
            SetHealthBarSize();
        }
        else if (Mathf.Abs(maxHealth - _maxHealthOld) > 0.001f)
        {
            _widthOld = _healthBar.GetComponent<RectTransform>().sizeDelta.x;
            _maxHealthOld = maxHealth;
            _maxHealthDisplayed = maxHealth;
        }
    }

    public void TakeDamage(int dmg)
    {
        SetHealthAmount(_health - dmg);
        if (_health <= 0)
        {
            OnHealthDepletion?.Invoke();
        }
    }

    private void SetHealthBarColorAndValue()
    {
        if (_slider)
        {
            _slider.maxValue = _maxHealthDisplayed;
            _slider.value = _healthDisplayed;
            float healthNormalized = _healthDisplayed / _maxHealthDisplayed;
            Color color = healthNormalized > halfHealthColorPosition
                ? Color.Lerp(halfHealthColor, fullHealthColor,
                    (healthNormalized - halfHealthColorPosition) * (1f / (1 - halfHealthColorPosition)))
                : Color.Lerp(zeroHealthColor, halfHealthColor, healthNormalized * (1f / halfHealthColorPosition));
            _slider.fillRect.GetComponent<Image>().color = color;
        }
    }

    private void SetHealthBarSize()
    {
        Vector2 currentSize = _healthBar.GetComponent<RectTransform>().sizeDelta;
        float widthDisplayed = _maxHealthDisplayed * _widthOld / _maxHealthOld;
        _healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(widthDisplayed, currentSize.y);
        SetHealthBarColorAndValue();
    }

    private void SetMaxHealthAmount(int newMaxHealth, bool withAnimation = true)
    {
        maxHealth = Mathf.Clamp(newMaxHealth, 0, newMaxHealth);
        if (withAnimation)
        {
            if (_timerSize >= 0)
            {
                _maxHealthOld = _maxHealthDisplayed;
                _widthOld = _healthBar.GetComponent<RectTransform>().sizeDelta.x;
            }

            _timerSize = changeMaxHealthAnimLength;
        }
    }

    private void SetHealthAmount(int health, bool withAnimation = true)
    {
        _health = Mathf.Clamp(health, 0, maxHealth);
        if (withAnimation)
        {
            if (_timerHealth >= 0)
            {
                _healthOld = _healthDisplayed;
            }

            _timerHealth = changeHealthAnimLength;
        }
    }

    public void LoadHealth(int currentHealth, int maximumHealth)
    {
        SetMaxHealthAmount(maximumHealth, false);
        SetHealthAmount(currentHealth, false);
    }

    public int GetHealthAmount()
    {
        return _health;
    }

    public int GetMaxHealthAmount()
    {
        return maxHealth;
    }

    public void Heal()
    {
        SetHealthAmount(maxHealth);
    }

    public void IncreaseCurrentHealth(int amount)
    {
        SetHealthAmount(_health + amount);
    }

    public void IncreaseMaximumHealth(int amount)
    {
        SetMaxHealthAmount(maxHealth + amount);
        Heal();
    }

    public bool HasFullHealth()
    {
        return _health == maxHealth;
    }
}