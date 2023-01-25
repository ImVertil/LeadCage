using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private Dictionary<Image, TextMeshProUGUI> _slots = new();
    private bool _isShown;

    void Start()
    {
        List<Transform> slotTransforms = new();

        // gotta change that, for now it works :^)
        foreach(var t in _inventoryPanel.GetComponentsInChildren<Transform>().ToList())
        {
            if (t.name.StartsWith("Slot"))
                slotTransforms.Add(t);
        }

        foreach (Transform slotTransform in slotTransforms)
        {
            _slots.Add(slotTransform.gameObject.GetComponentsInChildren<Image>()[1], slotTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>());
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
        if(_isShown)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        _isShown = !_isShown;
    }

    public void UpdateItems()
    {
        for(int i=0; i<_slots.Count; i++)
        {
            var slotsEntry = _slots.ElementAt(i);
            if (_items.Count > i)
            {
                var itemsEntry = _items.ElementAt(i);
                slotsEntry.Key.sprite = itemsEntry.Value.icon;
                slotsEntry.Value.text = itemsEntry.Value.itemName;
                slotsEntry.Key.enabled = true;
                slotsEntry.Value.enabled = true;
            }
            else
            {
                slotsEntry.Key.sprite = null;
                slotsEntry.Value.text = "";
                slotsEntry.Key.enabled = false;
                slotsEntry.Value.enabled = false;
            }
        }
    }

    public void AddItem(InteractPickup pickup)
    {
        _items.Add(pickup.gameObject, pickup.item); 
        pickup.gameObject.SetActive(false);
        UpdateItems();
    }

    // This may not be very well optimized :|
    public void DropItem(Image slot)
    {
        // Get the index on which the item is located in slot dictionary
        int index = 0;
        for(int i=0; i<_slots.Count; i++)
        {
            if (_slots.ElementAt(i).Key == slot)
            { index = i; break; }
        }

        // Get the Key/Value at index from items dictionary, set it to active and set its position
        GameObject itemObject = _items.ElementAt(index).Key;
        itemObject.SetActive(true);
        itemObject.transform.position = new Vector3(0, 0, 0);

        // Remove entry from the dictionary
        _items.Remove(itemObject);

        // Update items in inventory
        UpdateItems();
    }
}