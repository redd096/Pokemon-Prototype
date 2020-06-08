using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Image imageTransition = default;
    [SerializeField] float timeFadeOut = 1;
    [SerializeField] float timeFadeIn = 1;
    [SerializeField] string nameNewScene = "Game";

    Coroutine fade;

    private void Start()
    {
        //set active
        imageTransition.gameObject.SetActive(true);

        //then start fade out
        fade = StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float delta = 0;

        while (delta < 1)
        {
            imageTransition.FadeOut(ref delta, timeFadeOut);
            yield return null;
        }

        fade = null;
    }

    public void StartGame()
    {
        //check is finished coroutine
        if (fade == null)
            fade = StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float delta = 0;

        while (delta < 1)
        {
            imageTransition.FadeIn(ref delta, timeFadeIn);
            yield return null;
        }

        //load new scene
        SceneLoader.instance.LoadNewScene(nameNewScene);
    }
}
