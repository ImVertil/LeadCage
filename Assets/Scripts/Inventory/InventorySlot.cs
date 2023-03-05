using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class InventorySlot : MonoBehaviour
{
    private Image _slotImageComponent;
    private TextMeshProUGUI _slotTextComponent;

    public Sprite icon
    {
        get { return _slotImageComponent.sprite; }
        set { _slotImageComponent.sprite = value; }
    }
    public string text
    {
        get { return _slotTextComponent.text; }
        set { _slotTextComponent.text = value; }
    }

    public void EnableSlot()
    {
        _slotImageComponent.enabled = true;
        _slotTextComponent.enabled = true;
    }

    public void DisableSlot()
    {
        _slotImageComponent.enabled = false;
        _slotTextComponent.enabled = false;
    }
}
