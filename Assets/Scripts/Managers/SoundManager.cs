using System.Collections;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] float speedFadeOut = 0.5f;
    [SerializeField] float speedFadeIn = 0.3f;

    AudioSource backgroundAudioSource;
    Coroutine fadeOutFadeIn;

    #region private API

    void CreateAudioSource()
    {
        backgroundAudioSource = gameObject.AddComponent<AudioSource>();
    }

    AudioSource GetAudioSource()
    {
        //create audio source if null
        if (backgroundAudioSource == null)
            CreateAudioSource();

        //return audio source
        return backgroundAudioSource;
    }

    IEnumerator FadeOutFadeIn(AudioSource audioSource, AudioClip clip, float volume, bool loop)
    {
        //fade out
        if (audioSource.isPlaying)
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= speedFadeOut * Time.deltaTime;
                yield return null;
            }
        }

        //set final volume
        audioSource.volume = 0;

        //set new clip and loop
        audioSource.clip = clip;
        audioSource.loop = loop;

        //play if not already playing
        if (audioSource.isPlaying == false)
        {
            audioSource.Play();
        }

        //fade in
        while (audioSource.volume < volume)
        {
            audioSource.volume += speedFadeIn * Time.deltaTime;
            yield return null;
        }

        //set final volume
        audioSource.volume = volume;

        fadeOutFadeIn = null;
    }

    #endregion

    /// <summary>
    /// Start audio clip for background. Can set volume and loop
    /// </summary>
    public void StartBackgroundMusic(AudioClip clip, float volume = 1, bool loop = false, bool fade = false)
    {
        //be sure to have audio source
        GetAudioSource();

        //if fade, start music with fade in
        if (fade)
        {
            //stop if running
            if (fadeOutFadeIn != null)
                StopCoroutine(fadeOutFadeIn);

            //start coroutine
            fadeOutFadeIn = StartCoroutine(FadeOutFadeIn(backgroundAudioSource, clip, volume, loop));

            return;
        }

        //else simply start music from this audio source
        StartMusic(backgroundAudioSource, clip, volume, loop);
    }

    /// <summary>
    /// Start audio clip. Can set volume and loop
    /// </summary>
    public static void StartMusic(AudioSource audioSource, AudioClip clip, float volume = 1, bool loop = false)
    {
        //be sure to have audio source
        if (audioSource == null)
            return;

        //change only if different clip (so we can have same music in different scenes without stop)
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.loop = loop;

            audioSource.Play();
        }
    }
}