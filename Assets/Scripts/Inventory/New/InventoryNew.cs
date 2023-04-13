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
            _itemPool = new ObjectPool<GameObject>(CreateItemObject, OnGetItemObject, OnReturnItemObject, OnDestroyItemObject, true, _poolSize, _poolSize + 4);
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
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    [SerializeField] private int _poolSize = 28;
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
        /*
         * LINQ - 0.4111ms
         * FOREACH - 0.0554ms
         * ^ Pierwsze podniesienie, nastêpne podniesienia ~0.0022ms w obydwu przypadkach
        */
        //InventorySlotNew slot = _inventoryItems.FirstOrDefault(s => s.Value == null).Key;
        InventorySlotNew slot = GetFirstEmptySlot(ref _inventoryItems);
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

    public void EquipItem(InventorySlotNew slot)
    {
        InventorySlotNew weaponSlot = GetFirstEmptySlot(ref _equippedWeapons);
        if (weaponSlot != null)
        {
            _equippedWeapons[weaponSlot] = _inventoryItems[slot];
            _inventoryItems[slot] = null;
            UpdateItems();
            ShowItemDetails(weaponSlot);
        }
    }

    public void UnequipItem(InventorySlotNew slot)
    {
        InventorySlotNew itemSlot = GetFirstEmptySlot(ref _inventoryItems);
        if (itemSlot != null)
        {
            _inventoryItems[itemSlot] = _equippedWeapons[slot];
            _equippedWeapons[slot] = null;
            UpdateItems();
            ShowItemDetails(itemSlot);
        }
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
                case ItemType.MovementItem:
                    _equipButton.onClick.RemoveAllListeners();
                    if (_equippedWeapons.ContainsKey(slot) || _equippedMovementItems.ContainsKey(slot))
                    {
                        _equipButtonText.text = "Unequip";
                        _dropButton.interactable = false;
                        _equipButton.onClick.AddListener(delegate { UnequipItem(slot); });
                    }
                    else
                    {
                        _equipButton.onClick.AddListener(delegate { EquipItem(slot); });
                    }
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

    public Item GetPrimaryWeapon()
    {
        return _equippedWeapons.ElementAt(0).Value;
    }

    public Item GetSecondaryWeapon()
    {
        return _equippedWeapons.ElementAt(1).Value;
    }

    private Item GetItemFromContainer(InventorySlotNew slot)
    {
        if (_equippedWeapons.ContainsKey(slot)) return _equippedWeapons[slot];
        else if (_equippedMovementItems.ContainsKey(slot)) return _equippedMovementItems[slot];
        else return _inventoryItems[slot];
    }

    private InventorySlotNew GetFirstEmptySlot(ref Dictionary<InventorySlotNew, Item> container)
    {
        foreach(var entry in container)
        {
            if (entry.Value == null)
                return entry.Key;
        }
        return null;
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