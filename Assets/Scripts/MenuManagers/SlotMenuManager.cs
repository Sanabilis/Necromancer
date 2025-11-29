using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotMenuManager : MonoBehaviour
{
    [SerializeField] private Color _bossKilledSaveColor;
    [SerializeField] private GameObject _sceneTransition;
    [SerializeField] private GameObject[] _slots;
    [SerializeField] private GameObject _deleteSlotDialog;
    private bool _deleteDialogOpened;
    private int _deleteSlotNumber;
    private GameObject[] _continueButtons;
    private GameObject[] _newGameButtons;
    private GameObject[] _deleteButtons;
    private GameObject[] _emptySlotText;
    private GameObject[] _stats;
    public GameObject imageOverlay;
    private GameObject _defaultButton;

    void Awake()
    {
        MenuInputManager.Instance.OnInputChanged += OnInputChanged;
        MenuInputManager.Instance.OnExitPressed += () =>
        {
            if (!_deleteDialogOpened)
            {
                BackToMainMenu();
            }
            else
            {
                CloseDeleteSlotDialog();
            }
        };

        Init();
        AddListeners();
        LoadData();
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

    public void BackToMainMenu()
    {
        _sceneTransition.GetComponent<SceneTransition>().Fade("MainMenu");
    }

    public void StartNewGame(int saveNumber)
    {
        _sceneTransition.GetComponent<SceneTransition>().Fade("GameScene");
        StaticData.LoadGame = false;
        StaticData.SaveNumber = saveNumber;
    }

    public void ContinueGame(int saveNumber)
    {
        _sceneTransition.GetComponent<SceneTransition>().Fade("GameScene");
        StaticData.LoadGame = true;
        StaticData.SaveNumber = saveNumber;
    }

    public void OpenDeleteSlotDialog(int saveNumber)
    {
        _deleteDialogOpened = true;
        _deleteSlotDialog.SetActive(true);
        imageOverlay.SetActive(true);
        _deleteSlotNumber = saveNumber;
        OnInputChanged(MenuInputManager.Instance.mouseActive);
    }

    public void CloseDeleteSlotDialog()
    {
        _deleteDialogOpened = false;
        _deleteSlotDialog.SetActive(false);
        imageOverlay.SetActive(false);
        OnInputChanged(MenuInputManager.Instance.mouseActive);
    }

    public void DeleteGameFromSlot()
    {
        GameSavesHandler.DeleteAllData(_deleteSlotNumber);
        LoadGameSlotData(_deleteSlotNumber);
        CloseDeleteSlotDialog();
    }

    private void Init()
    {
        _deleteSlotDialog.SetActive(false);
        _continueButtons = new GameObject[_slots.Length];
        _newGameButtons = new GameObject[_slots.Length];
        _deleteButtons = new GameObject[_slots.Length];
        _emptySlotText = new GameObject[_slots.Length];
        _stats = new GameObject[_slots.Length];

        for (int i = 0; i < _slots.Length; i++)
        {
            _continueButtons[i] = _slots[i].transform.GetChild(1).gameObject;
            _newGameButtons[i] = _slots[i].transform.GetChild(2).gameObject;
            _deleteButtons[i] = _slots[i].transform.GetChild(3).gameObject;
            _emptySlotText[i] = _slots[i].transform.GetChild(4).gameObject;
            _stats[i] = _slots[i].transform.GetChild(5).gameObject;
        }
    }

    private void AddListeners()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            int slot = i + 1;
            _continueButtons[i].GetComponent<Button>().onClick.AddListener(() => ContinueGame(slot));
            _newGameButtons[i].GetComponent<Button>().onClick.AddListener(() => StartNewGame(slot));
            _deleteButtons[i].GetComponent<Button>().onClick.AddListener(() => OpenDeleteSlotDialog(slot));
        }
    }

    private void LoadData()
    {
        for (int slot = 1; slot <= _slots.Length; slot++)
        {
            LoadGameSlotData(slot);
        }
    }

    private void LoadGameSlotData(int slotNumber)
    {
        int i = slotNumber - 1;
        bool saveExists =
            File.Exists(StaticData.SavePath + StaticData.PlayerSaveFile + slotNumber + StaticData.SaveExtension);

        _continueButtons[i].SetActive(saveExists);
        _newGameButtons[i].SetActive(!saveExists);
        _deleteButtons[i].SetActive(saveExists);
        _emptySlotText[i].SetActive(!saveExists);
        _stats[i].SetActive(saveExists);

        if (saveExists)
        {
            LoadSlotStats(slotNumber, _stats[i].gameObject);
        }

        if (slotNumber == 1)
        {
            _defaultButton = _slots[0].transform.GetChild(saveExists ? 1 : 2).gameObject;
        }
    }

    private void LoadSlotStats(int slot, GameObject statsGameObject)
    {
        GameData gameData = GameSavesHandler.LoadGameData(slot);
        if (gameData != null)
        {
            double timeInSeconds = gameData.playTime;
            string time = TimeSpan.FromSeconds(timeInSeconds).ToString("");
            TextMeshProUGUI timeText = statsGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            timeText.text = time;

            int deaths = gameData.deaths;
            TextMeshProUGUI deathsText = statsGameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            deathsText.text = "Deaths: " + deaths;

            int killed = gameData.killed;
            TextMeshProUGUI killedText = statsGameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            killedText.text = "Killed: " + killed;

            if (gameData.bossKilled)
            {
                timeText.color = _bossKilledSaveColor;
                deathsText.color = _bossKilledSaveColor;
                killedText.color = _bossKilledSaveColor;
            }
        }
    }

    void Destroy()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _continueButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            _newGameButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            _deleteButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    private void OnInputChanged(bool mouseActive)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (!mouseActive)
        {
            EventSystem.current.SetSelectedGameObject(_deleteDialogOpened
                ? _deleteSlotDialog.transform.GetChild(2).gameObject
                : _defaultButton);
        }
    }

    #region SINGLETON PATTERN

    private static SlotMenuManager _instance;

    public static SlotMenuManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SlotMenuManager>();
            }

            return _instance;
        }
    }

    #endregion
}