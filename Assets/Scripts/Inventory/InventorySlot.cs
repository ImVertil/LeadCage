using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _slotImageComponent;
    [SerializeField] private TextMeshProUGUI _slotTextComponent;
    [SerializeField] private Button _slotRemoveButton;

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
        if(_slotTextComponent.text != null)
        {
            _slotRemoveButton.gameObject.SetActive(true);
        }
    }

    public void DisableSlot()
    {
        _slotImageComponent.enabled = false;
        _slotTextComponent.enabled = false;
        _slotRemoveButton.gameObject.SetActive(false);
    }

    public void AssignComponents()
    {
        _slotImageComponent = GetComponentsInChildren<Image>()[1];
        _slotTextComponent = GetComponentInChildren<TextMeshProUGUI>();
        _slotRemoveButton = GetComponentInChildren<Button>();
        _slotRemoveButton.gameObject.SetActive(false);
    }
}
