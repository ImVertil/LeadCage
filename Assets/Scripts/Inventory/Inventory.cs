using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region SINGLETON
    public static Inventory Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private GameObject _inventoryPanel;
    private Dictionary<GameObject, Item> _items = new();
    private Dictionary<InventorySlot, Item> _slots = new();
    private bool _isShown;

    void Start()
    {
        foreach (var slot in _inventoryPanel.GetComponentsInChildren<InventorySlot>().ToList())
        {
            slot.AssignComponents();
            _slots.Add(slot, null);    
        }

        _isShown = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown(Controls.INVENTORY))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        _inventoryPanel.SetActive(!_isShown);
        if (_isShown)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            PlayerController.CanMove = true;
            PlayerController.CanMoveCamera = true;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            PlayerController.CanMove = false;
            PlayerController.CanMoveCamera = false;
        }
        _isShown = !_isShown;
    }

    public void UpdateItems()
    {
        foreach(var slot in _slots)
        {
            if(slot.Value != null)
            {
                // Assings proper item name and icon to the slot's text and image component
                slot.Key.icon = slot.Value.icon;
                slot.Key.text = slot.Value.itemName;
                slot.Key.EnableSlot();
            }
            else
            {
                // Clears the inventory slot from item name and icon
                slot.Key.icon = null;
                slot.Key.text = "";
                slot.Key.DisableSlot();
            }
        }
    }

    public void AddItem(InteractPickup pickup)
    {
        // Add an entry with GameObject and Item ref, disable the game object
        _items.Add(pickup.gameObject, pickup.item);
        pickup.gameObject.SetActive(false);

        // Find first free inventory slot and assign it an item that we have just picked up
        InventorySlot key = _slots.FirstOrDefault(slot => slot.Value == null).Key;
        _slots[key] = pickup.item;

        UpdateItems();
    }

    public void DropItem(InventorySlot slot)
    {
        // Find the object associated with the item in the slot that we're dropping it from, set it to active and set its position in front of player
        GameObject itemObject = _items.FirstOrDefault(item => item.Value == _slots[slot]).Key;
        itemObject.SetActive(true);
        itemObject.transform.localPosition = new Vector3(0, 1, 0); // this will be the "in front of player" position later on
        //itemObject.transform.position = new Vector3(0, 1, 0);

        // Remove the object/item pair from dictionary and set the slots item to null as there is nothing there after we dropped an item
        _items.Remove(itemObject);
        _slots[slot] = null;

        UpdateItems();
    }
}