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
            _itemPool = new ObjectPool<GameObject>(CreateItemObject, OnGetItemObject, OnReturnItemObject, OnDestroyItemObject, true, _poolSize, _poolSize + 4);
            _itemDropTransform = Camera.main.transform;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    [SerializeField] private int _poolSize = 28;
    [SerializeField] private Transform _itemDropTransform;

    private ObjectPool<GameObject> _itemPool;
    public Dictionary<InventorySlotNew, Item> _inventoryItems { get; private set; } = new();
    public Dictionary<InventorySlotNew, Item> _equippedWeapons { get; private set; } = new();
    public Dictionary<InventorySlotNew, Item> _equippedMovementItems { get; private set; } = new();

    public void AddItem(InteractPickup pickup)
    {
        /*
         * LINQ - 0.4111ms
         * FOREACH - 0.0554ms
         * ^ Pierwsze podniesienie, nastêpne podniesienia ~0.0022ms w obydwu przypadkach
        */
        //InventorySlotNew slot = _inventoryItems.FirstOrDefault(s => s.Value == null).Key;
        InventorySlotNew slot = GetFirstEmptySlot(_inventoryItems);
        _inventoryItems[slot] = pickup.item;
        _itemPool.Release(pickup.gameObject);
        InventoryEvents.InventoryUpdate();
        //UpdateItems();
    }

    public void DropItem(InventorySlotNew slot)
    {
        GameObject obj = _itemPool.Get();
        AssignItemData(obj, _inventoryItems[slot]);
        _inventoryItems[slot] = null;
        InventoryEvents.InventoryUpdate();
        //UpdateItems();
    }

    public void EquipItem(InventorySlotNew slot)
    {
        InventorySlotNew weaponSlot = GetFirstEmptySlot(_equippedWeapons);
        if (weaponSlot != null)
        {
            _equippedWeapons[weaponSlot] = _inventoryItems[slot];
            _inventoryItems[slot] = null;
            InventoryEvents.InventoryUpdate();
            //UpdateItems();
            InventoryEvents.ShowItemDetails(weaponSlot);
            //ShowItemDetails(weaponSlot);
        }
    }

    public void UnequipItem(InventorySlotNew slot)
    {
        InventorySlotNew itemSlot = GetFirstEmptySlot(_inventoryItems);
        if (itemSlot != null)
        {
            _inventoryItems[itemSlot] = _equippedWeapons[slot];
            _equippedWeapons[slot] = null;
            InventoryEvents.InventoryUpdate();
            InventoryEvents.ShowItemDetails(itemSlot);
            //UpdateItems();
            //ShowItemDetails(itemSlot);
        }
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
        InventoryEvents.InventoryUpdate();
    }

    public void ShowItemDetails(InventorySlotNew slot)
    {
        InventoryEvents.ShowItemDetails(slot);
    }

    private void AssignItemData(GameObject obj, Item item)
    {
        obj.GetComponent<InteractPickup>().item = item;
        obj.GetComponent<MeshFilter>().mesh = item.modelMesh;
        obj.GetComponent<MeshRenderer>().material = item.modelMaterial;
        obj.name = item.itemName;
    }

    public void AddWeaponSlot(InventorySlotNew slot)
    {
        _equippedWeapons.Add(slot, null);
    }
    
    public void AddMovementItemSlot(InventorySlotNew slot)
    {
        _equippedMovementItems.Add(slot, null);
    }

    public void AddInventorySlot(InventorySlotNew slot)
    {
        _inventoryItems.Add(slot, null);
    }

    public Item GetPrimaryWeapon()
    {
        return _equippedWeapons.ElementAt(0).Value;
    }

    public Item GetSecondaryWeapon()
    {
        return _equippedWeapons.ElementAt(1).Value;
    }

    public Item GetItemFromContainer(InventorySlotNew slot)
    {
        if (_equippedWeapons.ContainsKey(slot)) return _equippedWeapons[slot];
        else if (_equippedMovementItems.ContainsKey(slot)) return _equippedMovementItems[slot];
        else return _inventoryItems[slot];
    }

    private InventorySlotNew GetFirstEmptySlot(Dictionary<InventorySlotNew, Item> container)
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