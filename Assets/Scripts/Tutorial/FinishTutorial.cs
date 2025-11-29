using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FinishTutorial : MonoBehaviour
{
    public PopUpMessage popup = null;
    private bool triggered;
    private bool _playerWantsToInteract;

    private InputMain _controls;

    void Awake()
    {
        _controls = new InputMain();

        _controls.Player.Interact.performed += _ => _playerWantsToInteract = true;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerWantsToInteract && triggered && !GameManager.Instance.IsGamePaused())
        {
            OnCheckpointTriggered();
        }

        _playerWantsToInteract = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            triggered = true;
            if (popup) popup.Display();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            triggered = false;
            if (popup) popup.Hide();
        }
    }

    void OnCheckpointTriggered()
    {
        GameManager.Instance.FinishTutorial();
    }

    void OnEnable()
    {
        _controls.Enable();
    }

    void OnDisable()
    {
        _controls.Disable();
    }
}