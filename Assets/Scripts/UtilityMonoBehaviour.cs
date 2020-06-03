using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UtilityMonoBehaviour : Singleton<UtilityMonoBehaviour>
{
    /// <summary>
    /// Write a text letter by letter
    /// </summary>
    public void WriteLetterByLetter(Text textToSet, string value, float timeBetweenChar, float skipSpeed, System.Action onEndWrite = null, bool canSkip = true)
    {
        StartCoroutine(WriteLetterByLetter_Coroutine(textToSet, value, timeBetweenChar, skipSpeed, onEndWrite, canSkip));
    }

    IEnumerator WriteLetterByLetter_Coroutine(Text textToSet, string value, float timeBetweenChar, float skipSpeed, System.Action onEndWrite, bool canSkip)
    {
        //foreach char in value
        for(int i = 0; i < value.Length; i++)
        {
            //set current text
            textToSet.text += value[i];

            //check if skip speed or normal speed
            float speed = canSkip && Input.anyKey ? skipSpeed : timeBetweenChar;

            //wait before new char
            yield return new WaitForSeconds(speed);
        }

        //wait until press any key
        while(!Input.anyKeyDown)
        {
            yield return null;
        }

        //call a function on end write
        onEndWrite?.Invoke();
    }
}
