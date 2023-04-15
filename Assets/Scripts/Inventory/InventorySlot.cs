using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _slotImageComponent;

    public Sprite icon
    {
        get { return _slotImageComponent.sprite; }
        set { _slotImageComponent.sprite = value; }
    }

    public void EnableSlot()
    {
        _slotImageComponent.enabled = true;
    }

    public void DisableSlot()
    {
        _slotImageComponent.enabled = false;
    }

    public void AssignComponents()
    {
        _slotImageComponent = GetComponentsInChildren<Image>()[1];
    }
}
