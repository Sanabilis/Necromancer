using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    public bool HasAssignedQuest;
    public bool WasQuestComleted;

    [SerializeField]
    private GameObject quests;

    [SerializeField]
    private string questType;

    [SerializeField]
    public Dialog PostQuestCompletedDialog;

    [SerializeField]
    public Dialog QuestCompletedDialog;

    [SerializeField]
    public Dialog QuestInProgressDialog;

    private Quest AssignedQuest;


    public override void Interact()
    {

        if(!HasAssignedQuest && !WasQuestComleted)
        {
            base.Interact();
            //AssignQuest();
        }
        else if(HasAssignedQuest && !WasQuestComleted)
        {
            CheckQuest();
        }
        else
        {
            DialogSystem.Instance.AddNewDialog(PostQuestCompletedDialog, npcName, this);
            DialogSystem.Instance.CreateDialog();
            DialogSystem.Instance.shouldReceiveInput = true;
        }
    }

    public override void DialogFinished()
    {
        //base.DialogFinished();
        if (!HasAssignedQuest && !WasQuestComleted)
        {
            AssignQuest();
        }
    }

    void AssignQuest()
    {
        HasAssignedQuest = true;
        AssignedQuest = (Quest)quests.AddComponent(System.Type.GetType(questType));
        AssignedQuest.Npc = this;
    }

    void CheckQuest()
    {
        if (AssignedQuest.Completed)
        {
            WasQuestComleted = true;
            HasAssignedQuest = false;
            DialogSystem.Instance.AddNewDialog(QuestCompletedDialog, npcName, this);
            DialogSystem.Instance.CreateDialog();
            DialogSystem.Instance.shouldReceiveInput = true;

        }
        else
        {
            DialogSystem.Instance.AddNewDialog(QuestInProgressDialog, npcName, this);
            DialogSystem.Instance.CreateDialog();
            DialogSystem.Instance.shouldReceiveInput = true;
        }
    }

    public string GetQuestType()
    {
        return questType;
    }

    public void LoadAssignedQuest(Quest quest)
    {
        HasAssignedQuest = true;
        AssignedQuest = quest;
    }
}
