using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utility
{
    /// <summary>
    /// Use inside a coroutine, for fade in
    /// </summary>
    public static void FadeIn(this Image image, ref float delta, float duration)
    {
        image.Fade(ref delta, 0, 1, duration);
    }

    /// <summary>
    /// Use inside a coroutine, for fade out
    /// </summary>
    public static void FadeOut(this Image image, ref float delta, float duration)
    {
        image.Fade(ref delta, 1, 0, duration);
    }

    static void Fade(this Image image, ref float delta, float from, float to, float duration)
    {
        //speed based to duration
        delta += Time.deltaTime / duration;

        //set alpha from to
        float alpha = Mathf.Lerp(from, to, delta);
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }
}
