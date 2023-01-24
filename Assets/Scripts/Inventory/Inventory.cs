using System.Collections;
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
    //private Dictionary<Item, GameObject> _items = new();
    private Dictionary<GameObject, Item> _items = new();
    private Dictionary<Image, TextMeshProUGUI> _slots = new();
    private bool _isShown;

    void Start()
    {
        List<Transform> slotTransforms = new();

        // gotta change that, for now it works :^)
        foreach(var t in _inventoryPanel.GetComponentsInChildren<Transform>().ToList())
        {
            if (t.name != "Image" && t.name != "ItemName" && t.name != "Panel")
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
        if (Input.GetKeyDown(KeyCode.Tab))
            ToggleInventory();
    }

    public void ToggleInventory()
    {
        _inventoryPanel.SetActive(!_isShown);
        _isShown = !_isShown;
    }

    public void UpdateItems()
    {
        for(int i=0; i<_slots.Count; i++)
        {
            var slotsEntry = _slots.ElementAt(i);
            if (_items.Count >= i+1)
            {
                var itemsEntry = _items.ElementAt(i);
                //slotsEntry.Key.sprite = itemsEntry.Key.icon;
                //slotsEntry.Value.text = itemsEntry.Key.itemName;
                slotsEntry.Key.sprite = itemsEntry.Value.icon;
                slotsEntry.Value.text = itemsEntry.Value.itemName;
                slotsEntry.Key.enabled = true;
                slotsEntry.Value.enabled = true;
            }
            else
            {
                slotsEntry.Key.sprite = null;
                slotsEntry.Value.text = null;
                slotsEntry.Key.enabled = false;
                slotsEntry.Value.enabled = false;
            }
        }
    }

    public void AddItem(InteractPickup pickup)
    {
        //_items.Add(pickup.item, pickup.gameObject); 
        _items.Add(pickup.gameObject, pickup.item); 
        pickup.gameObject.SetActive(false);
        UpdateItems();
    }

    public void DropItem(Item item)
    {
        // Get the GameObject assigned to the Item
        //GameObject itemObject = _items[item];
        GameObject itemObject = _items.FirstOrDefault(x => x.Value == item).Key;
        itemObject.SetActive(true);
        itemObject.transform.position = new Vector3(0,0,0);

        // Remove the entry from dictionary
        //_items.Remove(item);
        _items.Remove(_items.FirstOrDefault(x => x.Value == item).Key);
        UpdateItems();
    }
}
