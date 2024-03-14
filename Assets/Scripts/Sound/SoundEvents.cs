using System;

public class SoundEvents
{
    public static Action OnVolumeChanged;
    public static Action<bool> OnGamePaused;

    public static void VolumeChanged() => OnVolumeChanged?.Invoke();
    public static void GamePaused(bool b) => OnGamePaused?.Invoke(b);
}
