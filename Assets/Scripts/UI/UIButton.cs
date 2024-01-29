using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySound(Sound.MenuButtonHover);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySound(Sound.MenuButtonPress);
    }
}
