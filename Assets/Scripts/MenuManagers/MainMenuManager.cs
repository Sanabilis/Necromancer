using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public GameObject sceneTransition;
    public GameObject quitDialog;
    public GameObject imageOverlay;
    public GameObject defaultButton;

    private bool _quitDialogOpened;

    void Awake()
    {
        MenuInputManager.Instance.OnInputChanged += OnInputChanged;
        MenuInputManager.Instance.OnExitPressed += () =>
        {
            if (!_quitDialogOpened)
            {
                OpenQuitDialog();
            }
            else
            {
                CloseQuitDialog();
            }
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        quitDialog.SetActive(false);
        imageOverlay.SetActive(false);
        OnInputChanged(StaticData.MouseActive);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OpenQuitDialog()
    {
        _quitDialogOpened = true;
        quitDialog.SetActive(true);
        imageOverlay.SetActive(true);
        OnInputChanged(MenuInputManager.Instance.mouseActive);
    }

    public void CloseQuitDialog()
    {
        _quitDialogOpened = false;
        quitDialog.SetActive(false);
        imageOverlay.SetActive(false);
        OnInputChanged(MenuInputManager.Instance.mouseActive);
    }

    public void StartGame()
    {
        sceneTransition.GetComponent<SceneTransition>().Fade("SlotMenu");
    }

    public void StartTutorial()
    {
        StaticData.SaveNumber = 0;
        StaticData.LoadGame = false;
        StaticData.Tutorial = true;
        sceneTransition.GetComponent<SceneTransition>().Fade("TutorialScene");
    }

    public void OpenOptionsMenu()
    {
        sceneTransition.GetComponent<SceneTransition>().Fade("OptionsMenu");
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void OnInputChanged(bool mouseActive)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (!mouseActive)
        {
            EventSystem.current.SetSelectedGameObject(_quitDialogOpened
                ? quitDialog.transform.GetChild(2).gameObject
                : defaultButton);
        }
    }

    #region SINGLETON PATTERN

    private static MainMenuManager _instance;

    public static MainMenuManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MainMenuManager>();
            }

            return _instance;
        }
    }

    #endregion
}