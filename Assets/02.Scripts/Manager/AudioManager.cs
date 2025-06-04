using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : BehaviourSingleton<AudioManager>
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioMixerGroup _bgmMixerGroup;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;

    // 각종 사운드들
    [Header("Audios")]
    [SerializeField] private List<AudioClip> BGMList;
    [SerializeField] private List<AudioClip> PlayerAudioList;
    [SerializeField] private List<AudioClip> EnemyAudioList;
    [SerializeField] private List<AudioClip> DynamicRuneAudioList;
    [SerializeField] private List<AudioClip> UIAudioList;

    [Header("Audio Pool")]
    public int PoolSize = 30;
    public GameObject AudioSourceChildObject;
    private List<AudioSource> audioSourceList = new List<AudioSource>();

    public AudioSource BGMAudioSource;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            var source = Instantiate(AudioSourceChildObject, transform.position, Quaternion.identity, gameObject.transform).GetComponent<AudioSource>();
            audioSourceList.Add(source);
        }

        DontDestroyOnLoad(gameObject);
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in audioSourceList)
        {
            if (!source.isPlaying) return source;
        }
        return null;
    }

    public bool CheckCurrentBGM(int index)
    {
        if (BGMAudioSource.resource == BGMList[index]) return true;
        return false;
    }

    public void SetBGMVolume(float sliderValue)
    {
        float volume = Mathf.Log10(sliderValue <= 0.001f ? 0.001f : sliderValue) * 20;
        _mixer.SetFloat("BGM", volume);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float volume = Mathf.Log10(sliderValue <= 0.001f ? 0.001f : sliderValue) * 20;
        _mixer.SetFloat("SFX", volume);
    }

    public void PlayBGM(int index, float fadeTime = 2f)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeToClip(index, fadeTime));
    }

    private IEnumerator FadeToClip(int index, float fadeTime)
    {
        float currentVolume;
        _mixer.GetFloat("BGM", out currentVolume);

        // 1. 볼륨 줄이기
        yield return StartCoroutine(SetMixerVolume("BGM", currentVolume, -80f, fadeTime / 2f));

        // 2. 클립 교체 후 재생
        BGMAudioSource.resource = BGMList[index];
        BGMAudioSource.Play();

        // 3. 볼륨 다시 올리기
        yield return StartCoroutine(SetMixerVolume("BGM", -80f, currentVolume, fadeTime / 2f));
    }

    private IEnumerator SetMixerVolume(string exposedParam, float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float value = Mathf.Lerp(from, to, elapsed / duration);
            _mixer.SetFloat(exposedParam, value);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _mixer.SetFloat(exposedParam, to);
    }

    public void PlayEnemyAudio(EnemyType enemyType, EnemyAudioType audioType, bool isLoop = false)
    {
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.outputAudioMixerGroup = _sfxMixerGroup;
        audioSource.loop = isLoop;

        audioSource.resource = EnemyAudioList[(int)audioType];
        audioSource.Play();
    }

    public void StopEnemyAudio(EnemyAudioType audioType)
    {
        foreach (AudioSource source in audioSourceList)
        {
            if (source.resource == EnemyAudioList[(int)audioType] && source.isPlaying)
            {
                source.Stop();
                source.loop = false;
            }
        }
    }

    public void PlayUIAudio(UIAudioType audioType)
    {
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.outputAudioMixerGroup = _sfxMixerGroup;

        audioSource.resource = UIAudioList[(int)audioType];
        audioSource.Play();
    }

    public void PlayPlayerAudio(PlayerAudioType audioType)
    {
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.outputAudioMixerGroup = _sfxMixerGroup;

        audioSource.resource = PlayerAudioList[(int)audioType];
        audioSource.Play();
    }

    public void PlayDynamicRuneAudio(DynamicRuneAudioType audioType)
    {
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.outputAudioMixerGroup = _sfxMixerGroup;

        audioSource.resource = DynamicRuneAudioList[(int)audioType];
        audioSource.Play();
    }
}
