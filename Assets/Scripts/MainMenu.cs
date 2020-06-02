using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Image imageTransition = default;
    [SerializeField] float timeFadeOut = 1;
    [SerializeField] float timeFadeIn = 1;

    Coroutine fade;

    private void Start()
    {
        imageTransition.gameObject.SetActive(true);

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

    IEnumerator FadeIn()
    {
        float delta = 0;

        while (delta < 1)
        {
            imageTransition.FadeIn(ref delta, timeFadeIn);
            yield return null;
        }

        SceneLoader.instance.LoadNewScene("Game");
    }

    public void StartGame()
    {
        if(fade == null)
            fade = StartCoroutine(FadeIn());
    }
}
