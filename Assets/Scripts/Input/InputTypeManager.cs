using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputType
{
    MouseAndKeyboard,
    Gamepad,
    Unknown
}

public class InputTypeManager : MonoBehaviour
{
    [HideInInspector] public InputType inputType = InputType.Unknown;
    public event Action<InputType> OnInputChanged;

    private InputMain _controls;

    void Awake()
    {
        _controls = new InputMain();
        _controls.Player.Jump.started += UpdateCurrentControlScheme;
        _controls.Player.Dash.started += UpdateCurrentControlScheme;
        _controls.Player.Block.started += UpdateCurrentControlScheme;
        _controls.Player.Attack.started += UpdateCurrentControlScheme;
        _controls.Player.Movement.performed += UpdateCurrentControlScheme;
        _controls.Player.Interact.started += UpdateCurrentControlScheme;
        _controls.Player.Pause.started += UpdateCurrentControlScheme;
        inputType = StaticData.InputType;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void UpdateCurrentControlScheme(InputAction.CallbackContext context)
    {
        InputType oldInputType = inputType;
        if (_controls.KeyboardandmouseScheme.SupportsDevice(context.control.device))
        {
            inputType = InputType.MouseAndKeyboard;
        }
        else if (_controls.GamepadScheme.SupportsDevice(context.control.device))
        {
            inputType = InputType.Gamepad;
        }
        else
        {
            inputType = InputType.Unknown;
        }

        if (oldInputType != inputType)
        {
            StaticData.InputType = inputType;
            OnInputChanged?.Invoke(inputType);
        }

        if (context.control.device != Mouse.current)
        {
            MouseManager.HideCursor();
        }
    }

    void OnEnable()
    {
        _controls.Enable();
    }

    void OnDisable()
    {
        _controls.Disable();
    }

    #region SINGLETON PATTERN

    private static InputTypeManager _instance;

    public static InputTypeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<InputTypeManager>();
            }

            return _instance;
        }
    }

    #endregion
}