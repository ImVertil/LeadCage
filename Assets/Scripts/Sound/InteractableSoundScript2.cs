using UnityEngine;

public class InteractableSoundScript2 : MonoBehaviour, IInteractable
{
    private int a;
    void Update()
    {
        // XD
        transform.Translate(0f, 0f, 5f * Time.deltaTime * a);

        if (transform.position.z <= 0 || transform.position.z == 7)
        {
            a = 1;
        }
        if(transform.position.z >= 14) 
        {
            a = -1;
        }
    }

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
        //SoundManager.Instance.PlaySound(Sound.Countdown, new Vector3(-30,60,0));
        SoundManager.Instance.PlaySound(Sound.Countdown, gameObject.transform);
    }
}