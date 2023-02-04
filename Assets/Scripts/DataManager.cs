using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class DataManager
{
    public static float Volume { get { return volume; } set { volume = value; onVolumeChange(value); } }
    static float volume = 0.4f;
    static Action<float> onVolumeChange = (float f) => { };

    public static void RegisterVolume(Action<float> action)
    {
        onVolumeChange += action;
    }

    public static void UnregisterVolume(Action<float> action)
    {
        onVolumeChange -= action;
    }
}
