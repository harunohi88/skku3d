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
    [SerializeField] private List<AudioClip> BasicEnemyAudioList;

    [Header("Audio Pool")]
    public int PoolSize = 20;
    private ObjectPool<AudioSource> AudioPool;
    public AudioSource AudioSourceChildObject;

    public AudioSource BGMAudioSource;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        AudioPool = new ObjectPool<AudioSource>(AudioSourceChildObject, PoolSize, transform);
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

    public void PlayResourceAudio(ResourceType type, Vector3 position, bool isDone)
    {
        //AudioSource audioSource = GetAvailableAudioSource();
        //audioSource.outputAudioMixerGroup = _sfxMixerGroup;
        //audioSource.spatialBlend = 1;
        //audioSource.transform.position = position;
        //if (type == ResourceType.Tree)
        //{
        //    if (isDone) audioSource.resource = TreeAudioList[1];
        //    else audioSource.resource = TreeAudioList[0];
        //}
        //else if (type == ResourceType.Rock)
        //{
        //    if (isDone) audioSource.resource = RockAudioList[1];
        //    else audioSource.resource = RockAudioList[0];
        //}
        //audioSource.Play();
    }

}
