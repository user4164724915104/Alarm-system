using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Alarm : MonoBehaviour
{
    [SerializeField] private Sensor _sensor;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _fadeDuration;
    private float _zeroVolume = 0f;
    private float _maxVolume = 1f;
    private Coroutine _currentFade;

    private void Start()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
        _audioSource.volume = _zeroVolume;
    }

    private void OnEnable()
    {
        _sensor.OnTriggerEvent += OnEventEnter;
    }

    private void OnDisable()
    {
        _sensor.OnTriggerEvent -= OnEventEnter;
    }

    private void OnEventEnter(bool stopAfterFade)
    {
        if (!stopAfterFade) 
        { 
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }

            if (_currentFade != null)
            {
                StopCoroutine(_currentFade);
            }
            _currentFade = StartCoroutine(FadeVolume(_zeroVolume, _maxVolume, _fadeDuration, stopAfterFade));
        }

        if (stopAfterFade)
        {
            if (_currentFade != null)
            {
                StopCoroutine(_currentFade);
            }
            _currentFade = StartCoroutine(FadeVolume(_audioSource.volume, _zeroVolume, _fadeDuration, stopAfterFade));
        }
    }

    private IEnumerator FadeVolume(float from, float to, float duration, bool stopAfterFade)
    {
        float elapsed = _zeroVolume;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        _audioSource.volume = to;

        if (stopAfterFade && to == _zeroVolume)
        {
            _audioSource.Stop();
            _audioSource.volume = _zeroVolume;
        }
    }
}
