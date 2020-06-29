using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UtilityMonoBehaviour : Singleton<UtilityMonoBehaviour>
{
    [SerializeField] float timeBetweenChar = 0.05f;
    [SerializeField] float skipSpeed = 0.01f;

    Coroutine isWriting;

    #region private API

    /// <summary>
    /// When press to skip, accelerate speed
    /// </summary>
    IEnumerator WriteLetterByLetter_SkipAccelerate_Coroutine(Text textToSet, string value, System.Action onEndWrite, float speed, bool canSkip, bool wait)
    {
        textToSet.text = string.Empty;

        //foreach char in value
        for (int i = 0; i < value.Length; i++)
        {
            //set current text
            textToSet.text += value[i];

            //wait before new char
            float startTime = Time.time;
            float speedChar = speed > 0 ? speed : timeBetweenChar;
            float waitTime = startTime + speedChar;

            while (waitTime > Time.time)
            {
                //if skip, wait skipSpeed instead of timeBetweenChar
                if (canSkip && Input.anyKey)
                {
                    float speedSkipChar = speed > 0 ? speed : skipSpeed;
                    waitTime = startTime + speedSkipChar;
                }

                yield return null;
            }

        }

        //if wait or when player skip text
        if (wait)
        {
            //wait until press any key down
            while (!Input.anyKeyDown)
            {
                yield return null;
            }
        }

        //call a function on end write
        onEndWrite?.Invoke();

        isWriting = null;
    }

    /// <summary>
    /// When press to skip, set immediatly all text
    /// </summary>
    IEnumerator WriteLetterByLetter_SkipImmediatly_Coroutine(Text textToSet, string value, System.Action onEndWrite, bool canSkip, bool wait)
    {
        bool skipped = false;
        textToSet.text = string.Empty;

        //foreach char in value
        for (int i = 0; i < value.Length; i++)
        {
            //set current text
            textToSet.text += value[i];

            //wait before new char
            float waitTime = Time.time + timeBetweenChar;

            while (waitTime > Time.time)
            {
                //if skip, set immediatly all text
                if (canSkip && Input.anyKeyDown)
                {
                    textToSet.text = value;
                    skipped = true;
                    i = value.Length;                       //end for cycle
                    yield return new WaitForEndOfFrame();   //end of frame, so can wait again Input.anyKeyDown
                    break;
                }

                yield return null;
            }
        }

        //if wait or when player skip text
        if (wait || skipped)
        {
            //wait until press any key down
            while (!Input.anyKeyDown)
            {
                yield return null;
            }
        }

        //call a function on end write
        onEndWrite?.Invoke();

        isWriting = null;
    }

    #endregion

    /// <summary>
    /// Write a text letter by letter, then wait input. When press to skip, accelerate speed
    /// </summary>
    public void WriteLetterByLetterAndWait_SkipAccelerate(Text textToSet, string value, System.Action onEndWrite = null, float speed = 0, bool canSkip = true)
    {
        if (isWriting != null)
            StopCoroutine(isWriting);

        isWriting = StartCoroutine(WriteLetterByLetter_SkipAccelerate_Coroutine(textToSet, value, onEndWrite, speed, canSkip, true));
    }

    /// <summary>
    /// Write a text letter by letter, then wait input. When press to skip, set immediatly all text
    /// </summary>
    public void WriteLetterByLetterAndWait_SkipImmediatly(Text textToSet, string value, System.Action onEndWrite = null, bool canSkip = true)
    {
        if (isWriting != null)
            StopCoroutine(isWriting);

        isWriting = StartCoroutine(WriteLetterByLetter_SkipImmediatly_Coroutine(textToSet, value, onEndWrite, canSkip, true));
    }

    /// <summary>
    /// Write a text letter by letter. When press to skip, accelerate speed
    /// </summary>
    public void WriteLetterByLetter_SkipAccelerate(Text textToSet, string value, System.Action onEndWrite = null, float speed = 0, bool canSkip = true)
    {
        if (isWriting != null)
            StopCoroutine(isWriting);

        isWriting = StartCoroutine(WriteLetterByLetter_SkipAccelerate_Coroutine(textToSet, value, onEndWrite, speed, canSkip, false));
    }

    /// <summary>
    /// Write a text letter by letter. When press to skip, set immediatly all text
    /// </summary>
    public void WriteLetterByLetter_SkipImmediatly(Text textToSet, string value, System.Action onEndWrite = null, bool canSkip = true)
    {
        if (isWriting != null)
            StopCoroutine(isWriting);

        isWriting = StartCoroutine(WriteLetterByLetter_SkipImmediatly_Coroutine(textToSet, value, onEndWrite, canSkip, false));
    }
}
