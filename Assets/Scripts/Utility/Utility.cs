﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utility
{
    public static List<T> CreateCopy<T>(this List<T> list)
    {
        List<T> newList = new List<T>();

        //add every element in new list
        foreach (T element in list)
        {
            newList.Add(element);
        }

        return newList;
    }

    public static T[] CreateCopy<T>(this T[] array)
    {
        T[] newArray = new T[array.Length];

        //add every element in new array
        for (int i = 0; i < array.Length; i++)
        {
            newArray[i] = array[i];
        }

        return newArray;
    }

    public static string Parse(string text, string value)
    {
        if (text.Contains("{0}"))
            return text.Replace("{0}", value);

        return text;
    }
}

public static class FadeUtility
{
    #region private API

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

    #endregion

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
}

public static class TextLetterByLetter
{
    /// <summary>
    /// Write a text letter by letter, then wait input. When press to skip, accelerate speed
    /// </summary>
    public static void WriteLetterByLetterAndWait_SkipAccelerate(this Text textToSet, string value, System.Action onEndWrite = null, float speed = 0, bool canSkip = true)
    {
        UtilityMonoBehaviour.instance.WriteLetterByLetterAndWait_SkipAccelerate(textToSet, value, onEndWrite, speed, canSkip);
    }

    /// <summary>
    /// Write a text letter by letter, then wait input. When press to skip, set immediatly all text
    /// </summary>
    public static void WriteLetterByLetterAndWait_SkipImmediatly(this Text textToSet, string value, System.Action onEndWrite = null, bool canSkip = true)
    {
        UtilityMonoBehaviour.instance.WriteLetterByLetterAndWait_SkipImmediatly(textToSet, value, onEndWrite, canSkip);
    }

    /// <summary>
    /// Write a text letter by letter. When press to skip, accelerate speed
    /// </summary>
    public static void WriteLetterByLetter_SkipAccelerate(this Text textToSet, string value, System.Action onEndWrite = null, float speed = 0, bool canSkip = true)
    {
        UtilityMonoBehaviour.instance.WriteLetterByLetter_SkipAccelerate(textToSet, value, onEndWrite, speed, canSkip);
    }

    /// <summary>
    /// Write a text letter by letter. When press to skip, set immediatly all text
    /// </summary>
    public static void WriteLetterByLetter_SkipImmediatly(this Text textToSet, string value, System.Action onEndWrite = null, bool canSkip = true)
    {
        UtilityMonoBehaviour.instance.WriteLetterByLetter_SkipImmediatly(textToSet, value, onEndWrite, canSkip);
    }
}
