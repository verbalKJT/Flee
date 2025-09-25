using UnityEngine;
using System;

public static class SoundManager
{
    public static event Action<Vector3, float> OnSoundEmitted;

    public static void EmitSound(Vector3 position, float range)
    {
        OnSoundEmitted?.Invoke(position, range);
    }
}