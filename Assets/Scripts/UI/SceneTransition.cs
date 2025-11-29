using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator transition;
    public float transitionLength = 1f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void Fade(String scene)
    {
        StartCoroutine(FadeTransition(scene));
    }

    private IEnumerator FadeTransition(String scene)
    {
        transition.SetTrigger("Fade");
        yield return new WaitForSeconds(transitionLength);
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}