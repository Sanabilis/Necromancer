using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogController : MonoBehaviour
{
    [SerializeField] GameObject dialogueCanvas;

    [SerializeField] TextMeshProUGUI textComponent;

    [SerializeField] Dialog dialogue;

    [SerializeField] Dialog startingDialogue;

    [SerializeField] Dialog defaultDialogue;


    private bool shouldListenForInput = false;

    private bool wantsToInteract;

    private InputMain _controls;

    void Awake()
    {
        _controls = new InputMain();
        _controls.Player.Interact.performed += _ => wantsToInteract = true;
        _controls.Player.Interact.canceled += _ => wantsToInteract = false;
    }

    private void Start()
    {
        dialogue = startingDialogue;
        textComponent.text = dialogue.GetDialogueText();
    }

    //private void OnTriggerEnter2D(Collider2D otherObject)
    //{
    //    if (otherObject.CompareTag("Player"))
    //    {
    //        textComponent.text = dialogue.GetDialogueText();
    //        dialogueCanvas.SetActive(true);
    //        shouldListenForInput = true;

    //    }
    //}

    //private void OnTriggerExit2D(Collider2D otherObject)
    //{
    //    if (otherObject.CompareTag("Player"))
    //    {
    //        textComponent.text = dialogue.GetDialogueText();
    //        dialogueCanvas.SetActive(false);
    //        shouldListenForInput = false;
    //    };
    //}

    private void Update()
    {
        if (shouldListenForInput)
        {
            ManageState();
        }
    }

    private void ManageState()
    {
        List<Dialog> nextStates;

        if (dialogue.HasNextDialogueStates())
        {
            nextStates = dialogue.GetNextDialogueStates();
        }
        else
        {
            nextStates = dialogue.GetNextDialogueState();
        }

        if (wantsToInteract)
        {
            wantsToInteract = false;
            if (!dialogue.IsDefaultState())
            {
                if (nextStates.Count > 0)
                {
                    dialogue = nextStates[0];
                    textComponent.text = dialogue.GetDialogueText();
                }
                else
                {
                    dialogueCanvas.SetActive(false);
                    dialogue = defaultDialogue;
                }
            }
            else
            {
                dialogueCanvas.SetActive(false);
            }
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