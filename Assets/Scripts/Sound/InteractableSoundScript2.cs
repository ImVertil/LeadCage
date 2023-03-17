
using UnityEngine;

public class InteractableSoundScript2 : MonoBehaviour, IInteractable
{
    public void OnStartLook()
    {
        Debug.Log("2 Start Look");
    }

    public void OnEndLook()
    {
        Debug.Log("2 End Look");
    }

    public void OnInteract()
    {
        SoundManager.Instance.PlaySound(Sound.Countdown, new Vector3(-30,60,0));
    }
}

