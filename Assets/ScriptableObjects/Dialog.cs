using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue")]
public class Dialog : ScriptableObject
{
    [TextArea(5,8)] // (minLines, maxLines)
    [SerializeField] 
    string dialogueText;

    // TODO: make dialog system work with nonlinear story[SerializeField]
    List<Dialog> nextDialogueStates;

    [SerializeField]
    Dialog nextDialogueState;

    [SerializeField]
    private bool isLastState;

    //[SerializeField] maybe will be used in the future should not be needed now
    private bool isDefaultState;

    public string GetDialogueText()
    {
        return dialogueText;
    }

    public bool HasNextDialogueStates()
    { 
        return nextDialogueStates != null && nextDialogueStates.Count > 0 ? true : false;
    }

    public List<Dialog> GetNextDialogueStates()
    {
        if (HasNextDialogueStates())
        {
            return nextDialogueStates;
        }
        else
        {
            return GetNextDialogueState();
        }
    }

    public List<Dialog> GetNextDialogueState()
    {
        var list =  new List<Dialog>();
        if (!isLastState && nextDialogueState != null)
        {
            list.Add(nextDialogueState);
        }        

        return list;
    }

    public bool IsDefaultState()
    {
        return isDefaultState;
    }

    public bool IsLastState()
    {
        return isLastState;
    }
}
