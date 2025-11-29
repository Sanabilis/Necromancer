using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : NPC
{
    private GameObject player;
    private InventorySO PlayerInventory;

    [SerializeField] private Dialog PlayerHealedDialog;

    [SerializeField] private Dialog NoItemDialog;

    [SerializeField] private Dialog PlayerHasFullHealthDialog;


    public ItemSO item;
    public int requiredItemAmount;

    public int healAmount;
    private bool _entryDialog = true;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        PlayerInventory = player.GetComponent<Player>().GetPlayerInventory();
    }

    // Start is called before the first frame update
    public override void Interact()
    {
        int itemAmount = HasItemInInventory(item);

        if (_entryDialog)
        {
            AddDialog(dialog);
            //_entryDialog = false;
        }
        else if (itemAmount > 0)
        {
            /*
            var damagable = player.GetComponent<Damageable>();
            if (!damagable.HasFullHealth())
            {
                AddDialog(PlayerHealedDialog);
                //var currentHealth = damagable.GetHealthAmount();
                //damagable.SetHealthAmount(currentHealth + healAmount);
                damagable.IncreaseCurrentHealth(healAmount);
                UseItem(item);
            }
            else
            {
                AddDialog(PlayerHasFullHealthDialog);
            }
            */
            var damagable = player.GetComponent<Damageable>();
            AddDialog(PlayerHealedDialog);
            damagable.IncreaseMaximumHealth(healAmount * itemAmount);

            UseItems(item, itemAmount);
        }
        else
        {
            AddDialog(NoItemDialog);
        }
    }

    public override void DialogFinished()
    {
        if (_entryDialog)
        {
            _entryDialog = false;
            shouldListenForInput = true;
            _inputListenTimer = DefaultInputTimer;
        }
    }

    private int HasItemInInventory(ItemSO item)
    {
        if (PlayerInventory)
        {
            return PlayerInventory.HasItem(item);
        }

        return 0;
    }

    private void AddDialog(Dialog passedDialog)
    {
        DialogSystem.Instance.AddNewDialog(passedDialog, npcName, this);
        DialogSystem.Instance.CreateDialog();


        // Debug.Log("Interact with NPC");
        DialogSystem.Instance.shouldReceiveInput = true;
    }

    private void UseItems(ItemSO item, int amount)
    {
        if (PlayerInventory)
        {
            PlayerInventory.RemoveItem(item, amount);
        }
    }
}