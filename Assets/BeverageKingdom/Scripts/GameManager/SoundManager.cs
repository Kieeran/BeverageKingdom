using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Mixer Groups")]
    [SerializeField] AudioMixerGroup _musicMixerGroup;
    [SerializeField] AudioMixerGroup _sfxMixerGroup;

    [Header("Audio Sources")]
    [SerializeField] AudioSource _audioSourceLoop;
    [SerializeField] AudioSource _audioSourceUnLoop;

    [Header("Audio Clips")]
    [SerializeField] AudioClip _homeMenuSE;
    [SerializeField] AudioClip _inGameSE;
    [SerializeField] AudioClip _meleeAttackSE;
    [SerializeField] AudioClip _thunderSoundFx;
    [SerializeField] AudioClip _iceSoundFx;
    [SerializeField] AudioClip _warningHotSpotFx;

    // Public properties to access audio clips
    public AudioClip HomeMenuSound => _homeMenuSE;
    public AudioClip InGameSound => _inGameSE;
    public AudioClip MeleeAttackSound => _meleeAttackSE;
    public AudioClip ThunderSound => _thunderSoundFx;
    public AudioClip IceSound => _iceSoundFx;
    public AudioClip WarningHotSpotSound => _warningHotSpotFx;

    public bool SoundToggle = true;
    public bool MusicToggle = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure AudioSources are set up
        if (_audioSourceLoop == null)
        {
            _audioSourceLoop = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("SoundManager: audioSourceLoop was not assigned, creating new AudioSource");
        }

        if (_audioSourceUnLoop == null)
        {
            _audioSourceUnLoop = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("SoundManager: audioSourceUnLoop was not assigned, creating new AudioSource");
        }

        // Set up AudioSource properties
        SetupAudioSource(_audioSourceLoop, true);
        SetupAudioSource(_audioSourceUnLoop, false);

        SoundToggle = true;
        MusicToggle = true;
    }

    public void ToggleSound(bool b)
    {
        SoundToggle = b;

        float volume = SoundToggle ? 0f : -80f; // -80f là mức gần như tắt tiếng
        _sfxMixerGroup.audioMixer.SetFloat("SFXVolume", volume);
    }

    public void ToggleMusic(bool b)
    {
        MusicToggle = b;

        float volume = MusicToggle ? 0f : -80f;
        _musicMixerGroup.audioMixer.SetFloat("MusicVolume", volume);
    }

    private void SetupAudioSource(AudioSource source, bool isLoop)
    {
        source.playOnAwake = false;
        source.loop = isLoop;
        source.spatialBlend = 0f; // 2D sound
        source.outputAudioMixerGroup = isLoop ? _musicMixerGroup : _sfxMixerGroup;
    }

    public void StopSound()
    {
        if (_audioSourceLoop != null && _audioSourceLoop.isPlaying)
        {
            _audioSourceLoop.Stop();
            _audioSourceLoop.loop = false;
        }
    }

    // public void PlaySound(AudioClip clip, bool loop)
    // {
    //     if (clip == null)
    //     {
    //         return;
    //     }

    //     if (loop)
    //     {
    //         if (_audioSourceLoop != null)
    //         {
    //             _audioSourceLoop.Stop();
    //             _audioSourceLoop.clip = clip;
    //             _audioSourceLoop.loop = true;
    //             _audioSourceLoop.Play();
    //         }
    //     }
    //     else
    //     {
    //         if (_audioSourceUnLoop != null)
    //         {
    //             _audioSourceUnLoop.clip = clip;
    //             _audioSourceUnLoop.loop = loop;
    //             _audioSourceUnLoop.Play();
    //         }
    //         else
    //         {
    //         }
    //     }
    // }

    public void PlayAudio(AudioClip clip, bool loop)
    {
        if (clip == null) return;

        if (loop)
        {
            if (_audioSourceLoop != null)
            {
                _audioSourceLoop.Stop();
                _audioSourceLoop.clip = clip;
                _audioSourceLoop.loop = true;
                _audioSourceLoop.Play();
            }
        }
        else
        {
            if (_audioSourceUnLoop != null && !_audioSourceUnLoop.isPlaying)
            {
                _audioSourceUnLoop.clip = clip;
                _audioSourceUnLoop.loop = false;
                _audioSourceUnLoop.Play();
            }
            else
            {
                // Tạo AudioSource mới nếu nguồn chính đang bận
                AudioSource extraSource = gameObject.AddComponent<AudioSource>();
                SetupAudioSource(extraSource, false);
                extraSource.clip = clip;
                extraSource.loop = false;
                extraSource.Play();

                // Tự động xóa AudioSource sau khi phát xong
                StartCoroutine(DestroyWhenFinished(extraSource));
            }
        }
    }

    IEnumerator DestroyWhenFinished(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        Destroy(source);
    }

    public void PlaySoundWithDelay(AudioClip clip, bool loop, float delay)
    {
        if (clip == null)
        {
            return;
        }
        StartCoroutine(PlayAfterDelay(clip, loop, delay));
    }

    private IEnumerator PlayAfterDelay(AudioClip clip, bool loop, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayAudio(clip, loop);
    }

    private void Start()
    {
        Camera.main.GetComponent<AudioListener>().enabled = true;
    }
}
