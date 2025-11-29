using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputManager : MonoBehaviour
{
    private Vector2 _mousePosition;
    [HideInInspector] public bool mouseActive;
    private bool _changeToKeys = false;
    public event Action OnExitPressed;
    public event Action OnPausePressed;
    public event Action<bool> OnInputChanged;

    private InputMain _controls;

    void Awake()
    {
        mouseActive = StaticData.MouseActive;
        _controls = new InputMain();
        _controls.Menu.Exit.performed += _ =>
        {
            if (mouseActive) _changeToKeys = true;
            OnExitPressed?.Invoke();
        };
        _controls.Player.Pause.performed += _ =>
        {
            if (mouseActive) _changeToKeys = true;
            OnPausePressed?.Invoke();
        };
        _controls.Menu.Navigation.performed += _ =>
        {
            if (mouseActive) _changeToKeys = true;
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        _mousePosition = Mouse.current.position.ReadValue();
    }

    // Update is called once per frame
    void Update()
    {
        if (_changeToKeys)
        {
            // navigation using keyboard / controller
            mouseActive = false;
            StaticData.MouseActive = false;
            _changeToKeys = false;
            OnInputChanged?.Invoke(mouseActive);

            MouseManager.HideCursor();
        }

        Vector2 currentMousePosition = Mouse.current.position.ReadValue();

        if (!mouseActive &&
            (Mathf.Abs(_mousePosition.x - currentMousePosition.x) > 2f ||
             Mathf.Abs(_mousePosition.y - currentMousePosition.y) > 2f))
        {
            // navigation using mouse
            mouseActive = true;
            StaticData.MouseActive = true;
            OnInputChanged?.Invoke(mouseActive);

            MouseManager.ShowCursor(); // Always show cursor when moved
        }

        _mousePosition = currentMousePosition;
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

    private static MenuInputManager _instance;

    public static MenuInputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MenuInputManager>();
            }

            return _instance;
        }
    }

    #endregion
}