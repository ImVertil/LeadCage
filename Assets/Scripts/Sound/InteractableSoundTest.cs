
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
        SoundManager.Instance.PlaySound(Sound.Test1, this.gameObject.transform.position);
        Debug.Log("Test2");
    }
}

