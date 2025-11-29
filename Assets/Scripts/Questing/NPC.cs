using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    protected const float DefaultInputTimer = 0.01f;

    //public string[] dialog;
    public Dialog dialog;
    public string npcName;
    public PopUpMessage popup;

    protected float _inputListenTimer = DefaultInputTimer;

    private bool wantsToInteract;

    private InputMain _controls;

    void Awake()
    {
        _controls = new InputMain();
        _controls.Player.Interact.performed += _ => wantsToInteract = true;
        _controls.Player.Interact.canceled += _ => wantsToInteract = false;
    }

    public override void Interact()
    {
        DialogSystem.Instance.AddNewDialog(dialog, npcName, this);
        DialogSystem.Instance.CreateDialog();

        Debug.Log("Interact with NPC");
        DialogSystem.Instance.shouldReceiveInput = true;
    }

    public override void DialogFinished()
    {
        if (tag == "LostSoul")
        {
            LostSoul ls = GetComponent<LostSoul>();
            ls.follow = true;
            ls.GiveItem();
            shouldListenForInput = false;
            popup.Hide();
        }
    }

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.CompareTag("Player"))
        {
            if (tag == "LostSoul" && GetComponent<LostSoul>().follow)
                shouldListenForInput = false;
            else
                shouldListenForInput = true;

            if (popup && (tag != "LostSoul" || !GetComponent<LostSoul>().follow))
            {
                popup.Display();
            }
        }
    }


    private void OnTriggerExit2D(Collider2D otherObject)
    {
        if (otherObject.CompareTag("Player"))
        {
            shouldListenForInput = false;
            DialogSystem.Instance.CloseDialog();
            if (popup)
            {
                popup.Hide();
            }
        }
    }

    private void Update()
    {
        if (shouldListenForInput && !(_inputListenTimer > 0f))
        {
            if (wantsToInteract)
            {
                Interact();
                shouldListenForInput = false;
            }
        }

        wantsToInteract = false;

        if (_inputListenTimer > 0f)
        {
            _inputListenTimer -= Time.deltaTime;
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
}