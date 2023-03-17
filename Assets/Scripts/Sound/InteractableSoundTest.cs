
using UnityEngine;

public class InteractableSoundTest : MonoBehaviour, IInteractable
{
    public void OnStartLook()
    {
        Debug.Log("Start Look");
    }

    public void OnEndLook()
    {
        Debug.Log("End Look");
    }

    public void OnInteract()
    {
        Debug.Log("Test");
        SoundManager.Instance.PlaySound(Sound.Countdown);
        Debug.Log("Test2");
    }
}

