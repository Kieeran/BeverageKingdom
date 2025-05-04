using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource audioSourceLoop;
    public AudioSource audioSourceUnLoop;

    public AudioClip HomeMenuSE;
    public AudioClip InGameSE;
    public AudioClip MeleeAttackSE;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StopSound()
    {
        if (audioSourceLoop != null && audioSourceLoop.isPlaying)
        {
            audioSourceLoop.Stop();
            audioSourceLoop.loop = false;
        }
    }

    public void PlaySound(AudioClip clip, bool loop)
    {
        if (loop == true)
        {
            audioSourceLoop.clip = clip;
            audioSourceLoop.loop = loop;
            audioSourceLoop.Play();
        }

        else
        {
            audioSourceUnLoop.clip = clip;
            audioSourceUnLoop.loop = loop;
            audioSourceUnLoop.Play();
        }
    }

    public void PlaySoundWithDelay(AudioClip clip, bool loop, float delay)
    {
        StartCoroutine(PlayAfterDelay(clip, loop, delay));
    }

    private IEnumerator PlayAfterDelay(AudioClip clip, bool loop, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySound(clip, loop);
    }
}
