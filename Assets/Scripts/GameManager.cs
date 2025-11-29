using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject restartDialog;
    private bool _restartDialogOpened;
    public GameObject quitDialog;
    private bool _quitDialogOpened;
    public GameObject imageOverlay;
    public GameObject sceneTransition;
    public GameObject player;
    public GameObject quests;
    public GameObject walls;
    public GameObject items;
    public PopUpMessage gameSavedMessage;
    public event Action OnRespawnEnemies;
    public event Action<bool> OnPause;
    private bool _isPaused;
    private bool _isQuitting;
    private int _enemiesKilledCount = 0;
    private GameObject _defaultButton;

    void Awake()
    {
        pauseMenu.SetActive(false);
        quitDialog.SetActive(false);
        imageOverlay.SetActive(false);

        MenuInputManager.Instance.OnInputChanged += OnInputChanged;
        MenuInputManager.Instance.OnPausePressed += () =>
        {
            if (!_isQuitting) PauseGame();
        };

        player.GetComponent<Damageable>().OnHealthDepletion += GameOver;

        CombatEvents.OnEnemyDeath += EnemyDied;

        StaticData.SessionStart = System.DateTime.Now;
        StaticData.OldPlayTime = 0;
        StaticData.OldDeathCount = 0;
        StaticData.OldKilledCount = 0;

        if (StaticData.LoadGame)
        {
            Load();
        }
        else
        {
            Save(player.transform.position);
        }

        _defaultButton = pauseMenu.transform.GetChild(1).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnInputChanged(StaticData.MouseActive);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CloseRestartDialog()
    {
        _restartDialogOpened = false;
        pauseMenu.SetActive(true);
        restartDialog.SetActive(false);
        OnInputChanged(MenuInputManager.Instance.mouseActive);
    }

    public void OpenRestartDialog()
    {
        _restartDialogOpened = true;
        pauseMenu.SetActive(false);
        restartDialog.SetActive(true);
        OnInputChanged(MenuInputManager.Instance.mouseActive);
    }

    public void RestartAtCheckpoint()
    {
        Time.timeScale = 1;
        _isQuitting = true;
        restartDialog.SetActive(false);
        StaticData.LoadGame = true;
        if (!StaticData.Tutorial)
        {
            sceneTransition.GetComponent<SceneTransition>().Fade("GameScene");
        }
        else
        {
            sceneTransition.GetComponent<SceneTransition>().Fade("TutorialScene");
        }
    }


    public void PauseGame()
    {
        _isPaused = !_isPaused;
        pauseMenu.SetActive(_isPaused);
        imageOverlay.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0 : 1;
        OnPause?.Invoke(_isPaused);
        OnInputChanged(MenuInputManager.Instance.mouseActive);
    }

    public void CloseQuitDialog()
    {
        _quitDialogOpened = false;
        pauseMenu.SetActive(true);
        quitDialog.SetActive(false);
        OnInputChanged(MenuInputManager.Instance.mouseActive);
    }

    public void OpenQuitDialog()
    {
        _quitDialogOpened = true;
        pauseMenu.SetActive(false);
        quitDialog.SetActive(true);
        OnInputChanged(MenuInputManager.Instance.mouseActive);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1;
        _isQuitting = true;
        _quitDialogOpened = false;
        quitDialog.SetActive(false);
        sceneTransition.GetComponent<SceneTransition>().Fade("MainMenu");
    }

    public void GameOver()
    {
        if (!_isQuitting)
        {
            if (!StaticData.Tutorial)
            {
                _isQuitting = true;
                SaveGameData(true);
                sceneTransition.GetComponent<SceneTransition>().Fade("GameOverMenu");
            }
            else
            {
                RestartAtCheckpoint();
            }
        }
    }

    public void FinishTutorial()
    {
        StaticData.Tutorial = false;
        ExitToMainMenu();
    }

    public bool IsGamePaused()
    {
        return _isPaused || _isQuitting;
    }

    public void EnemyDied(EnemyScript enemy)
    {
        ++_enemiesKilledCount;
        if (enemy.type == EnemyType.Boss) BossDied();
    }

    public void BossDied()
    {
        StaticData.BossKilled = true;
        Save(player.transform.position);
        if (gameSavedMessage != null) gameSavedMessage.Display();
    }

    public void RespawnEnemies()
    {
        OnRespawnEnemies?.Invoke();
    }

    public void Save(Vector3 playerLocation)
    {
        SavePlayer(playerLocation);
        SaveGameData(false);
        SaveQuestsData();
        SaveWallsData();
        SaveItemsData();
        SaveInventoryData();
    }

    private void Load()
    {
        LoadPlayer();
        LoadGameData();
        LoadQuestsData();
        LoadWallsData();
        LoadItemsData();
        LoadInventoryData();
    }

    private void SavePlayer(Vector3 playerLocation)
    {
        if (player != null)
        {
            GameSavesHandler.SavePlayerData(player, StaticData.SaveNumber, playerLocation);
        }
    }

    private void SaveGameData(bool died)
    {
        GameSavesHandler.SaveGameData(StaticData.SaveNumber, died, _enemiesKilledCount);
    }

    private void SaveQuestsData()
    {
        if (quests == null) return;
        GameSavesHandler.SaveQuestsData(StaticData.SaveNumber, quests);
    }

    private void SaveWallsData()
    {
        if (walls == null) return;
        GameSavesHandler.SaveWallsData(StaticData.SaveNumber, walls);
    }

    private void SaveItemsData()
    {
        if (items == null) return;
        GameSavesHandler.SaveItemsData(StaticData.SaveNumber, items);
    }

    private void SaveInventoryData()
    {
        GameSavesHandler.SaveInventoryData(StaticData.SaveNumber, player);
    }

    private void LoadPlayer()
    {
        PlayerData playerData = GameSavesHandler.LoadPlayerData(StaticData.SaveNumber);
        if (playerData == null) return;
        player.transform.position = new Vector3(playerData.position[0], playerData.position[1], playerData.position[2]);
        player.GetComponent<Damageable>().LoadHealth(playerData.health, playerData.maxHealth);
    }

    private void LoadGameData()
    {
        GameData gameData = GameSavesHandler.LoadGameData(StaticData.SaveNumber);
        if (gameData == null) return;
        StaticData.OldPlayTime = gameData.playTime;
        StaticData.OldDeathCount = gameData.deaths;
        StaticData.OldKilledCount = gameData.killed;
        StaticData.BossKilled = gameData.bossKilled;
    }

    private void LoadQuestsData()
    {
        if (quests == null) return;

        QuestsData questsData = GameSavesHandler.LoadQuestsData(StaticData.SaveNumber);
        if (questsData == null) return;

        QuestGiver[] npcs = FindObjectsOfType<QuestGiver>();
        foreach (QuestData questData in questsData.quests)
        {
            foreach (QuestGiver npc in npcs)
            {
                if (npc.npcName != questData.npcName) continue;
                Quest quest = (Quest) quests.AddComponent(System.Type.GetType(npc.GetQuestType()));
                npc.LoadAssignedQuest(quest);
                quest.LoadQuest(questData, npc);
                break;
            }
        }
    }

    private void LoadWallsData()
    {
        if (walls == null) return;

        WallsData wallsData = GameSavesHandler.LoadWallsData(StaticData.SaveNumber);
        if (wallsData == null) return;

        for (int i = 0; i < walls.transform.childCount; i++)
        {
            GameObject wallGO = walls.transform.GetChild(i).gameObject;
            if (!wallGO.TryGetComponent(out DestructibleWall wall))
            {
                break;
            }

            DestructibleType type = wall.type;
            WallData wallData = Array.Find(wallsData.wallsList, data =>
            {
                return type == data.type &&
                       Math.Abs(wallGO.transform.position.x - data.position[0]) <= 0.001f &&
                       Math.Abs(wallGO.transform.position.y - data.position[1]) <= 0.001f &&
                       Math.Abs(wallGO.transform.position.z - data.position[2]) <= 0.001f;
            });

            if (wallData == null)
            {
                Destroy(wallGO);
            }
        }
    }

    private void LoadItemsData()
    {
        if (items == null) return;

        ItemsData itemsData = GameSavesHandler.LoadItemsData(StaticData.SaveNumber);
        if (itemsData == null) return;

        for (int i = 0; i < items.transform.childCount; i++)
        {
            GameObject itemGO = items.transform.GetChild(i).gameObject;
            if (!itemGO.TryGetComponent(out Item item))
            {
                break;
            }

            ItemData itemData = Array.Find(itemsData.itemsList, data =>
            {
                return Math.Abs(itemGO.transform.position.x - data.position[0]) <= 0.001f &&
                       Math.Abs(itemGO.transform.position.y - data.position[1]) <= 0.001f &&
                       Math.Abs(itemGO.transform.position.z - data.position[2]) <= 0.001f;
            });

            if (itemData == null)
            {
                Destroy(itemGO);
            }
        }
    }

    private void LoadInventoryData()
    {
        InventoryData inventoryData = GameSavesHandler.LoadInventoryData(StaticData.SaveNumber);
        if (inventoryData == null) return;

        List<InventorySlot> inventoryContainer = new List<InventorySlot>();
        foreach (InventorySlotData inventorySlotData in inventoryData.inventory)
        {
            InventorySlot inventorySlot = new InventorySlot(new ItemSO(), inventorySlotData.amount);
            inventorySlot.item.id = inventorySlotData.item.id;
            inventoryContainer.Add(inventorySlot);
        }

        InventorySO newInventory = new InventorySO {Container = inventoryContainer};
        player.GetComponent<Player>().LoadPlayerInventory(newInventory);
    }

    private void OnInputChanged(bool mouseActive)
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (!_isPaused || _isQuitting || mouseActive) return;

        if (!_quitDialogOpened && !_restartDialogOpened)
        {
            EventSystem.current.SetSelectedGameObject(_defaultButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(_quitDialogOpened
                ? quitDialog.transform.GetChild(2).gameObject
                : restartDialog.transform.GetChild(2).gameObject);
        }
    }

    #region SINGLETON PATTERN

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }

    #endregion
}