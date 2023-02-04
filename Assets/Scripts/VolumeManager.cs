using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.volume = DataManager.Volume;

        DataManager.RegisterVolume(adjustVolume);
    }

    private void OnDestroy()
    {
        DataManager.UnregisterVolume(adjustVolume);
    }

    public void adjustVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
