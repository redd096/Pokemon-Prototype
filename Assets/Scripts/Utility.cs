using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utility
{
    /// <summary>
    /// Use inside a coroutine (delta 0 to 1), for fade in (alpha from 0 to 1)
    /// </summary>
    public static void FadeIn(this Image image, ref float delta, float duration)
    {
        image.Fade(ref delta, 0, 1, duration);
    }

    /// <summary>
    /// Use inside a coroutine (delta 0 to 1), for fade out (alpha from 1 to 0)
    /// </summary>
    public static void FadeOut(this Image image, ref float delta, float duration)
    {
        image.Fade(ref delta, 1, 0, duration);
    }

    /// <summary>
    /// Use inside a coroutine (delta 0 to 1), for fade in (alpha from 0 to 1) with fillAmount
    /// </summary>
    public static void FadeIn_Fill(this Image image, ref float delta, float duration)
    {
        image.Fade_Fill(ref delta, 0, 1, duration);
    }

    /// <summary>
    /// Use inside a coroutine (delta 0 to 1), for fade out (alpha from 1 to 0) with fillAmount
    /// </summary>
    public static void FadeOut_Fill(this Image image, ref float delta, float duration)
    {
        image.Fade_Fill(ref delta, 1, 0, duration);
    }

    static void Fade(this Image image, ref float delta, float from, float to, float duration)
    {
        //speed based to duration
        delta += Time.deltaTime / duration;

        //set alpha from to
        float alpha = Mathf.Lerp(from, to, delta);
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    static void Fade_Fill(this Image image, ref float delta, float from, float to, float duration)
    {
        //speed based to duration
        delta += Time.deltaTime / duration;

        //set fill amout
        image.fillAmount = Mathf.Lerp(from, to, delta);
    }
}
