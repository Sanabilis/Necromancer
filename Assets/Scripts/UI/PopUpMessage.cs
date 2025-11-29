using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpMessage : MonoBehaviour
{
    private GameObject _keyboardAndMouseText;
    private GameObject _gamepadText;
    private bool _onlyDefaultText = true;
    public bool automaticallyDisappearing = false;
    private float _animLength = 1.5f;
    private float _animTimer = -1f;
    private bool _popUpDisplayed;

    void Awake()
    {
        _keyboardAndMouseText = transform.GetChild(0).gameObject;
        if (transform.childCount > 1)
        {
            _onlyDefaultText = false;
            _gamepadText = transform.GetChild(1).gameObject;
        }

        InputTypeManager.Instance.OnInputChanged += UpdateText;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateText(StaticData.InputType);
    }

    // Update is called once per frame
    void Update()
    {
        if (automaticallyDisappearing)
        {
            if (_animTimer > 0)
            {
                _animTimer -= Time.deltaTime;
            }
            else if (_popUpDisplayed)
            {
                Hide();
            }
        }
    }

    public void Display()
    {
        GetComponent<Animator>().SetTrigger("Start");
        _popUpDisplayed = true;
        if (automaticallyDisappearing) _animTimer = _animLength;
    }

    public void Hide()
    {
        GetComponent<Animator>().SetTrigger("End");
        _popUpDisplayed = false;
    }

    private void UpdateText(InputType inputType)
    {
        if (!_onlyDefaultText && inputType == InputType.Gamepad)
        {
            _gamepadText.SetActive(true);
            _keyboardAndMouseText.SetActive(false);
        }
        else
        {
            if (!_onlyDefaultText) _gamepadText.SetActive(false);
            _keyboardAndMouseText.SetActive(true);
        }
    }
}