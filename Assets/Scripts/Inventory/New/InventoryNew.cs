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
                slot.AssignComponents();
                _inventoryItems.Add(slot, null);
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

    [SerializeField] private int _poolSize = 20;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private Transform _itemDropTransform;

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
        {
            DropItem(_inventoryItems.ElementAt(0).Key);
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

    // TODO
    public void SortItems()
    {

    }

    private void AssignItemData(GameObject obj, Item item)
    {
        obj.GetComponent<InteractPickup>().item = item;
        obj.GetComponent<MeshFilter>().mesh = item.modelMesh;
        obj.GetComponent<MeshRenderer>().material = item.modelMaterial;
        obj.name = item.name;
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
