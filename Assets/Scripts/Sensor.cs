using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sensor : MonoBehaviour
{
    private bool _stopAfterFade;
    public event Action<bool> OnTriggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEvent?.Invoke(_stopAfterFade = false);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerEvent?.Invoke(_stopAfterFade = true);
    }
}