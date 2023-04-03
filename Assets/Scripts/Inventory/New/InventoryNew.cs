using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using TMPro;

public class InventoryNew : MonoBehaviour
{
    #region SINGLETON
    public static InventoryNew Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _itemPool = new ObjectPool<GameObject>(CreateItemObject, OnGetItemObject, OnReturnItemObject, OnDestroyItemObject, true, _poolSize, _poolSize + 5);
            foreach (var slot in _inventoryPanel.GetComponentsInChildren<InventorySlotNew>().ToList())
            {
                if(slot.transform.parent == _weaponsPanel.transform)
                {
                    _equippedWeapons.Add(slot, null);
                }
                else
                {
                    _inventoryItems.Add(slot, null);
                }
                slot.AssignComponents();
            }
            _isVisible = false;
            _itemDropTransform = Camera.main.transform;

            /* uwaga du�e XD tutaj
            TextMeshProUGUI[] texts = _inventoryDetailsPanel.GetComponentsInChildren<TextMeshProUGUI>();
            Image[] images = _inventoryDetailsPanel.GetComponentsInChildren<Image>();
            Button[] buttons = _inventoryDetailsPanel.GetComponentsInChildren<Button>();
            _itemName = texts[0];
            _itemIcon = images[2];
            _itemDescription = texts[3];
            _equipButton = buttons[0];
            _dropButton = buttons[1];
            _equipButtonText = texts[4];*/
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    [SerializeField] private int _poolSize = 20;
    [SerializeField] private GameObject _weaponsPanel;
    [SerializeField] private GameObject _movementItemsPanel;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private Transform _itemDropTransform;
    [SerializeField] private GameObject _inventoryDetailsPanel;

    [SerializeField] private TextMeshProUGUI _detailsItemName;
    [SerializeField] private Image _detailsItemIcon;
    [SerializeField] private TextMeshProUGUI _detailsItemDescription;
    [SerializeField] private Button _equipButton;
    [SerializeField] private TextMeshProUGUI _equipButtonText;
    [SerializeField] private Button _dropButton;

    private ObjectPool<GameObject> _itemPool;
    private Dictionary<InventorySlotNew, Item> _inventoryItems = new();
    private Dictionary<InventorySlotNew, Item> _equippedWeapons = new();
    private Dictionary<InventorySlotNew, Item> _equippedMovementItems = new();
    private bool _isVisible;

    void Update()
    {
        if (Input.GetButtonDown(Controls.INVENTORY))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.G))
            SortItems();
    }

    public void ToggleInventory()
    {
        if (_isVisible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            PlayerController.CanMove = true;
            PlayerController.CanMoveCamera = true;
            _inventoryPanel.SetActive(false);
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            PlayerController.CanMove = false;
            PlayerController.CanMoveCamera = false;
            _inventoryPanel.SetActive(true);
            _inventoryDetailsPanel.SetActive(false);
        }
        _isVisible = !_isVisible;
    }

    public void AddItem(InteractPickup pickup)
    {
        InventorySlotNew slot = _inventoryItems.FirstOrDefault(s => s.Value == null).Key;
        _inventoryItems[slot] = pickup.item;
        _itemPool.Release(pickup.gameObject);
        UpdateItems();
    }

    public void DropItem(InventorySlotNew slot)
    {
        GameObject obj = _itemPool.Get();
        AssignItemData(obj, _inventoryItems[slot]);
        _inventoryItems[slot] = null;
        UpdateItems();
    }

    // TODO
    public void EquipItem(InventorySlotNew slot)
    {
        Debug.Log("Equipped (surely)");
        InventorySlotNew weaponSlot = _equippedWeapons.FirstOrDefault(s => s.Value == null).Key;
        _equippedWeapons[weaponSlot] = _inventoryItems[slot];
        _inventoryItems[slot] = null;
        //_equipButtonText.text = "Equipped";
        //_equipButton.interactable = false;
        UpdateItems();
        ShowItemDetails(weaponSlot);
    }

    public void UnequipItem(InventorySlotNew slot)
    {
        Debug.Log("Unequipped (surely)");
        InventorySlotNew itemSlot = _inventoryItems.FirstOrDefault(s => s.Value == null).Key;
        _inventoryItems[itemSlot] = _equippedWeapons[slot];
        _equippedWeapons[slot] = null;
        UpdateItems();
        ShowItemDetails(itemSlot);
    }

    public void UpdateItems()
    {
        foreach (var slot in _equippedWeapons)
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

        foreach (var slot in _inventoryItems)
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

    public void SortItems()
    {
        List<InventorySlotNew> slots = new();
        Queue<Item> items = new();
        foreach(var entry in _inventoryItems)
        {
            slots.Add(entry.Key);
            if (entry.Value != null)
            {
                items.Enqueue(entry.Value);
            }
        }

        foreach(var key in slots)
        {
            if (items.Count == 0)
            {
                _inventoryItems[key] = null;
            }
            else
            {
                _inventoryItems[key] = items.Dequeue();
            }
        }
        UpdateItems();
    }

    // gotta rewrite the code a bit, it looks awful
    public void ShowItemDetails(InventorySlotNew slot)
    {
        Item item = GetItemFromContainer(slot);
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
                    _equipButton.onClick.RemoveAllListeners();
                    if (_equippedWeapons.ContainsKey(slot))
                    {
                        _equipButtonText.text = "Unequip";
                        _dropButton.interactable = false;
                        _equipButton.onClick.AddListener(delegate { UnequipItem(slot); });
                    }
                    else
                    {
                        _dropButton.interactable = true;
                        _equipButton.interactable = true;
                        _equipButton.onClick.AddListener(delegate { EquipItem(slot); });
                    }
                    break;

                case ItemType.MovementItem:
                    _equipButton.interactable = true;
                    break;
            }
            _dropButton.onClick.RemoveAllListeners();
            _dropButton.onClick.AddListener(delegate { DropItem(slot); });
        }
    }

    private void AssignItemData(GameObject obj, Item item)
    {
        obj.GetComponent<InteractPickup>().item = item;
        obj.GetComponent<MeshFilter>().mesh = item.modelMesh;
        obj.GetComponent<MeshRenderer>().material = item.modelMaterial;
        obj.name = item.itemName;
    }

    private Item GetItemFromContainer(InventorySlotNew slot)
    {
        Transform parent = slot.transform.parent;
        if (parent == _weaponsPanel.transform) return _equippedWeapons[slot];
        else if (parent == _movementItemsPanel.transform) return _equippedMovementItems[slot];
        else return _inventoryItems[slot];

        /*if (_equippedWeapons.ContainsKey(slot)) return _equippedWeapons[slot];
        else if (_equippedMovementItems.ContainsKey(slot)) return _equippedMovementItems[slot];
        else return _inventoryItems[slot];*/
    }

    #region OBJECT_POOL_METHODS
    private GameObject CreateItemObject()
    {
        GameObject gameObject = new GameObject("Item");
        return gameObject;
    }

    private void OnGetItemObject(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.position = _itemDropTransform.position + Vector3.forward; // change to in-front player pos later
    }

    private void OnReturnItemObject(GameObject obj)
    {
        obj.transform.parent = null;
        obj.SetActive(false);
    }

    private void OnDestroyItemObject(GameObject obj)
    {
        Destroy(obj);
    }
    #endregion
}
