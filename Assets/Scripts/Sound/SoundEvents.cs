using System;

public class SoundEvents
{
    public static Action OnVolumeChanged;

    public static void VolumeChanged() => OnVolumeChanged?.Invoke();
}
