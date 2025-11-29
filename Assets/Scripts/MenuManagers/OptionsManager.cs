using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsManager : MonoBehaviour
{
    public GameObject sceneTransition;
    public GameObject defaultButton;

    void Awake()
    {
        MenuInputManager.Instance.OnInputChanged += OnInputChanged;
        MenuInputManager.Instance.OnExitPressed += BackToMainMenu;
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
        sceneTransition.GetComponent<SceneTransition>().Fade("MainMenu");
    }

    private void OnInputChanged(bool mouseActive)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (!mouseActive) EventSystem.current.SetSelectedGameObject(defaultButton);
    }

    #region SINGLETON PATTERN

    private static OptionsManager _instance;

    public static OptionsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<OptionsManager>();
            }

            return _instance;
        }
    }

    #endregion
}