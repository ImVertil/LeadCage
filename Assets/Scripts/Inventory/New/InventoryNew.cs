using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

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
                //slot.AssignComponents();
                _inventoryItems.Add(slot, null);
            }
            _isVisible = false;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    [SerializeField] private int _poolSize = 20;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private Transform _itemDropPosition;

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
        _inventoryPanel.SetActive(!_isVisible);
        if (_isVisible)
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
        AssignItemData(_itemPool.Get(), _inventoryItems[slot]);
        UpdateItems();
    }

    public void UpdateItems()
    {
        foreach (var slot in _inventoryItems)
        {
            if (slot.Value != null)
            {
                // Assings proper item icon to the slot's image component
                slot.Key.icon = slot.Value.icon;
                slot.Key.EnableSlot();
            }
            else
            {
                // Clears the inventory slot from item icon
                slot.Key.icon = null;
                slot.Key.DisableSlot();
            }
        }
    }

    public void SortItems()
    {

    }

    private void AssignItemData(GameObject obj, Item item)
    {
        obj.GetComponent<InteractPickup>().item = item;
        obj.GetComponent<MeshFilter>().mesh = item.modelMesh;
    }

    #region OBJECT_POOL_METHODS
    private GameObject CreateItemObject()
    {
        GameObject gameObject = new GameObject("unnamed");
        return gameObject;
    }

    private void OnGetItemObject(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.position = new Vector3(0, 1, 0); // change to in-front player pos later
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
