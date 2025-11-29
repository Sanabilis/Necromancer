using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverManager : MonoBehaviour
{
    public GameObject sceneTransition;
    public GameObject defaultButton;

    void Awake()
    {
        MenuInputManager.Instance.OnInputChanged += OnInputChanged;
        MenuInputManager.Instance.OnExitPressed += QuitToMainMenu;
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


    public void QuitToMainMenu()
    {
        sceneTransition.GetComponent<SceneTransition>().Fade("MainMenu");
    }

    public void LoadFromCheckpoint()
    {
        StaticData.LoadGame = true;
        sceneTransition.GetComponent<SceneTransition>().Fade("GameScene");
    }

    private void OnInputChanged(bool mouseActive)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (!mouseActive) EventSystem.current.SetSelectedGameObject(defaultButton);
    }

    #region SINGLETON PATTERN

    private static GameOverManager _instance;

    public static GameOverManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameOverManager>();
            }

            return _instance;
        }
    }

    #endregion
}