using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//NB. If you want, you can remove pauseButton and movementMenu from the Canvas

public class UIManager : MonoBehaviour
{
    [Header("Image used for transitions")]
    [SerializeField] Image transitionImage = default;

    [Header("Pause")]
    [SerializeField] Button pauseButton = default;
    [SerializeField] Sprite pauseSprite = default;
    [SerializeField] Sprite resumeSprite = default;
    [SerializeField] GameObject pauseMenu = default;

    [Header("Movement")]
    [SerializeField] GameObject movementMenu = default;

    private void Start()
    {
        transitionImage.gameObject.SetActive(true);
        PauseMenu(false);
        ShowMovementMenu(true);
    }

    #region private API

    IEnumerator FadeIn_Coroutine(float duration, System.Action onEnd)
    {
        float delta = 0;

        while (delta < 1)
        {
            transitionImage.FadeIn_Fill(ref delta, duration);
            yield return null;
        }

        onEnd?.Invoke();
    }

    IEnumerator FadeOut_Coroutine(float duration, System.Action onEnd)
    {
        float delta = 0;

        while (delta < 1)
        {
            transitionImage.FadeOut_Fill(ref delta, duration);
            yield return null;
        }

        onEnd?.Invoke();
    }

    #endregion

    #region public API

    #region transition image

    public void FadeIn_Fill(float duration, System.Action onEnd)
    {
        StartCoroutine(FadeIn_Coroutine(duration, onEnd));
    }

    public void FadeOut_Fill(float duration, System.Action onEnd)
    {
        StartCoroutine(FadeOut_Coroutine(duration, onEnd));
    }

    public void FadeOut(ref float delta, float timeToFadeOut)
    {
        transitionImage.FadeOut(ref delta, timeToFadeOut);
    }

    public void ResetTransitionImage()
    {
        //reset alpha but remove fill amount, so player can't see image
        transitionImage.fillAmount = 0;
        Color imageColor = transitionImage.color;
        transitionImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, 1);
    }

    #endregion

    public void PauseMenu(bool pause)
    {
        pauseMenu.SetActive(pause);

        //change button sprite
        if(pauseButton)
            pauseButton.image.sprite = pause ? resumeSprite : pauseSprite;
    }

    public void ShowMovementMenu(bool show)
    {
        if(movementMenu)
            movementMenu.SetActive(show);
    }

    #endregion
}
