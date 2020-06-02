using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Image transitionImage;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    #region for states

    public void FadeIn(float duration, System.Action onEnd)
    {
        StartCoroutine(FadeIn_Coroutine(duration, onEnd));
    }

    public void FadeOut(float duration, System.Action onEnd)
    {
        StartCoroutine(FadeOut_Coroutine(duration, onEnd));
    }

    public void Wait(float duration, System.Action onEnd)
    {
        StartCoroutine(Wait_Coroutine(duration, onEnd));
    }

    IEnumerator FadeIn_Coroutine(float duration, System.Action onEnd)
    {
        float delta = 0;

        while(delta < 1)
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

    IEnumerator Wait_Coroutine(float duration, System.Action onEnd)
    {
        yield return new WaitForSeconds(duration);

        onEnd?.Invoke();
    }

    #endregion

    public void StartFight()
    {
        anim.SetTrigger("Fight");
    }

    public void StartMoving()
    {
        anim.SetTrigger("Moving");
    }
}
