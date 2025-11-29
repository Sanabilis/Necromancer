using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;


//SINGLETON
public class DialogSystem : MonoBehaviour
{
    public static DialogSystem Instance { get; set; }

    public GameObject dialogPanel;

    public string npcName;

    //public List<string> dialogs = new List<string>();

    public Dialog dialog;

    public bool shouldReceiveInput;


    TextMeshProUGUI dialogText;
    TextMeshProUGUI nameText;
    public int dialogIndex;

    public NPC dialogOwner;

    private InputMain _controls;

    void Awake()
    {
        _controls = new InputMain();
        _controls.Player.Interact.performed += _ =>
        {
            if (shouldReceiveInput) ContinueDialog();
        };

        dialogText = dialogPanel.transform.Find("DialogText").GetComponent<TextMeshProUGUI>();
        nameText = dialogPanel.transform.Find("NamePanel").Find("Name").GetChild(0).GetComponent<TextMeshProUGUI>();

        dialogPanel.SetActive(false);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        GameManager.Instance.OnPause += HideDialog;
    }

    private void HideDialog(bool hide)
    {
        dialogPanel.transform.parent.gameObject.SetActive(!hide);
    }

    public void AddNewDialog(Dialog newDialog, string npcName, NPC dialogOwner)
    {
        this.dialog = newDialog;
        this.npcName = npcName;
        this.dialogOwner = dialogOwner;
    }

    private void Update()
    {
    }

    public void CreateDialog()
    {
        dialogText.text = dialog.GetDialogueText();
        nameText.text = this.npcName;

        dialogPanel.SetActive(true);
    }

    public void CloseDialog()
    {
        shouldReceiveInput = false;
        dialogPanel.SetActive(false);
    }

    private void ContinueDialog()
    {
        if (!dialog.IsLastState())
        {
            dialog = dialog.GetNextDialogueStates().FirstOrDefault();
            if (dialog != null)
            {
                dialogText.text = dialog.GetDialogueText();
            }
            else
            {
                CloseDialog();
            }
        }
        else
        {
            CloseDialog();
            dialogOwner.DialogFinished();
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