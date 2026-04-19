using System.Collections;
using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] float fadeDuration;
    private float _zeroVolume = 0f;
    private float _maxVolume = 1f;
    private Coroutine currentFade;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        audioSource.volume = _zeroVolume;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        currentFade = StartCoroutine(FadeVolume(_zeroVolume, _maxVolume, fadeDuration));
    }

    void OnTriggerExit(Collider other)
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        currentFade = StartCoroutine(FadeVolume(audioSource.volume, _zeroVolume, fadeDuration, true));
    }

    IEnumerator FadeVolume(float from, float to, float duration, bool stopAfterFade = false)
    {
        float elapsed = _zeroVolume;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        audioSource.volume = to;

        if (stopAfterFade && to == _zeroVolume)
        {
            audioSource.Stop();
            audioSource.volume = _zeroVolume;
        }
    }
}