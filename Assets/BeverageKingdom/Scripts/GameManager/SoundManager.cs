using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSourceLoop;
    [SerializeField] private AudioSource audioSourceUnLoop;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip HomeMenuSE;
    [SerializeField] private AudioClip InGameSE;
    [SerializeField] private AudioClip MeleeAttackSE;
    [SerializeField] private AudioClip ThunderSoundFx;
    [SerializeField] private AudioClip IceSoundFx;

    // Public properties to access audio clips
    public AudioClip HomeMenuSound => HomeMenuSE;
    public AudioClip InGameSound => InGameSE;
    public AudioClip MeleeAttackSound => MeleeAttackSE;
    public AudioClip ThunderSound => ThunderSoundFx;
    public AudioClip IceSound => IceSoundFx;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure AudioSources are set up
        if (audioSourceLoop == null)
        {
            audioSourceLoop = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("SoundManager: audioSourceLoop was not assigned, creating new AudioSource");
        }

        if (audioSourceUnLoop == null)
        {
            audioSourceUnLoop = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("SoundManager: audioSourceUnLoop was not assigned, creating new AudioSource");
        }

        // Set up AudioSource properties
        SetupAudioSource(audioSourceLoop, true);
        SetupAudioSource(audioSourceUnLoop, false);
    }

    private void SetupAudioSource(AudioSource source, bool isLoop)
    {
        source.playOnAwake = false;
        source.loop = isLoop;
        source.spatialBlend = 0f; // 2D sound
        source.outputAudioMixerGroup = isLoop ? musicMixerGroup : sfxMixerGroup;
    }

    public void StopSound()
    {
        if (audioSourceLoop != null && audioSourceLoop.isPlaying)
        {
            audioSourceLoop.Stop();
            audioSourceLoop.loop = false;
            Debug.Log("SoundManager: Stopped loop sound");
        }
    }

    public void PlaySound(AudioClip clip, bool loop)
    {
        if (clip == null)
        {
            Debug.LogError("SoundManager: Attempted to play null AudioClip!");
            return;
        }

        if (loop)
        {
            if (audioSourceLoop != null)
            {
                audioSourceLoop.clip = clip;
                audioSourceLoop.loop = loop;
                audioSourceLoop.Play();
                Debug.Log($"SoundManager: Playing loop sound: {clip.name}");
            }
            else
            {
                Debug.LogError("SoundManager: audioSourceLoop is null!");
            }
        }
        else
        {
            if (audioSourceUnLoop != null)
            {
                audioSourceUnLoop.clip = clip;
                audioSourceUnLoop.loop = loop;
                audioSourceUnLoop.Play();
                Debug.Log($"SoundManager: Playing one-shot sound: {clip.name}");
            }
            else
            {
                Debug.LogError("SoundManager: audioSourceUnLoop is null!");
            }
        }
    }

    public void PlaySoundWithDelay(AudioClip clip, bool loop, float delay)
    {
        if (clip == null)
        {
            Debug.LogError("SoundManager: Attempted to play null AudioClip with delay!");
            return;
        }
        StartCoroutine(PlayAfterDelay(clip, loop, delay));
    }

    private IEnumerator PlayAfterDelay(AudioClip clip, bool loop, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySound(clip, loop);
    }

    private void Start()
    {
        Camera.main.GetComponent<AudioListener>().enabled = true;
    }
}
