using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource audioSource;

    public AudioClip HomeMenuSE;
    public AudioClip InGameSE;
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

    void Start()
    {
        PlaySoundWithDelay(HomeMenuSE, true, 1f);
    }

    public void PlaySound(AudioClip clip, bool loop)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
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
