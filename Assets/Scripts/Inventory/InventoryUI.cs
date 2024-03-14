using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _weaponsPanel;
    [SerializeField] private GameObject _movementItemsPanel;
    [SerializeField] private GameObject _inventoryDetailsPanel;

    [SerializeField] private TextMeshProUGUI _detailsItemName;
    [SerializeField] private Image _detailsItemIcon;
    [SerializeField] private TextMeshProUGUI _detailsItemDescription;
    [SerializeField] private Button _equipButton;
    [SerializeField] private TextMeshProUGUI _equipButtonText;
    [SerializeField] private Button _dropButton;

    private bool _isVisible;

    void Start()
    {
        foreach (var slot in _mainPanel.GetComponentsInChildren<InventorySlot>())
        {
            if (slot.transform.parent == _weaponsPanel.transform)
            {
                Inventory.Instance.AddWeaponSlot(slot);
            }
            else if (slot.transform.parent == _movementItemsPanel.transform)
            {
                Inventory.Instance.AddMovementItemSlot(slot);
            }
            else
            {
                Inventory.Instance.AddInventorySlot(slot);
            }
            slot.AssignComponents();
            
            _isVisible = false;
            InventoryEvents.OnInventoryUpdate += UpdateItems;
            InventoryEvents.OnShowItemDetails += ShowItemDetails;
            InputManager.current.InventoryToggleAction.performed += ToggleInventory;
        }
    }

    public void ToggleInventory(InputAction.CallbackContext ctx)
    {
        // REPLACE WITH PLAYER EVENTS WHEN THEY'RE ADDED
        if (PlayerController.IsInteracting)
            return;

        if (_isVisible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            InputManager.OnUnfreezeMovement();
            InputManager.OnEnableShooting();
            _mainPanel.SetActive(false);
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            InputManager.OnFreezeMovement();
            InputManager.OnDisableShooting();
            _mainPanel.SetActive(true);
            _inventoryDetailsPanel.SetActive(false);
        }
        _isVisible = !_isVisible;
    }

    public void UpdateItems()
    {
        foreach (var slot in Inventory.Instance._equippedWeapons)
        {
            if (slot.Value != null)
            {
                slot.Key.icon = slot.Value.icon;
                slot.Key.EnableSlot();
            }
            else
            {
                slot.Key.icon = null;
                slot.Key.DisableSlot();
            }
        }

        foreach (var slot in Inventory.Instance._inventoryItems)
        {
            if (slot.Value != null)
            {
                slot.Key.icon = slot.Value.icon;
                slot.Key.EnableSlot();
            }
            else
            {
                slot.Key.icon = null;
                slot.Key.DisableSlot();
            }
        }
        // we do that because we can't pick up items while in inventory and whenever we drop something we have to disable the panel cuz the item is not here.
        _inventoryDetailsPanel.SetActive(false);
    }

    public void ShowItemDetails(InventorySlot slot)
    {
        Item item = Inventory.Instance.GetItemFromContainer(slot);
        if (_isVisible && item != null)
        {
            _inventoryDetailsPanel.SetActive(true);
            _detailsItemName.text = item.itemName;
            _detailsItemIcon.sprite = item.icon;
            _detailsItemDescription.text = item.description;

            // By default we want the buttons to be active, rest is handled in the switch
            _equipButtonText.text = "Equip";
            _dropButton.interactable = true;
            _equipButton.interactable = true;

            switch (item.type)
            {
                case ItemType.Item:
                    _equipButton.interactable = false;
                    _equipButtonText.text = "Not equippable";
                    break;

                case ItemType.Weapon:
                case ItemType.MovementItem:
                    _equipButton.onClick.RemoveAllListeners();
                    if (Inventory.Instance._equippedWeapons.ContainsKey(slot) || Inventory.Instance._equippedMovementItems.ContainsKey(slot))
                    {
                        _equipButtonText.text = "Unequip";
                        _dropButton.interactable = false;
                        _equipButton.onClick.AddListener(delegate { Inventory.Instance.UnequipItem(slot); });
                    }
                    else
                    {
                        _equipButton.onClick.AddListener(delegate { Inventory.Instance.EquipItem(slot); });
                    }
                    break;
            }

            _dropButton.onClick.RemoveAllListeners();
            _dropButton.onClick.AddListener(delegate { Inventory.Instance.DropItem(slot); });
        }
    }

    public void SortItems()
    {
        Inventory.Instance.SortItems();
    }
}
