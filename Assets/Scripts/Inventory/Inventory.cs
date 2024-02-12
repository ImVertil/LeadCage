using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class Inventory : MonoBehaviour
{
    #region SINGLETON
    public static Inventory Instance { get; private set; }
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
    public Dictionary<InventorySlot, Item> _inventoryItems { get; private set; } = new();
    public Dictionary<InventorySlot, Item> _equippedWeapons { get; private set; } = new();
    public Dictionary<InventorySlot, Item> _equippedMovementItems { get; private set; } = new();

    private bool _firstTimeEquip = true;
    public void AddItem(InteractPickup pickup)
    {
        /*
         * LINQ - 0.4111ms
         * FOREACH - 0.0554ms
         * ^ Pierwsze podniesienie, nastêpne podniesienia ~0.0022ms w obydwu przypadkach
        */
        //InventorySlotNew slot = _inventoryItems.FirstOrDefault(s => s.Value == null).Key;
        InventorySlot slot = GetFirstEmptySlot(_inventoryItems);
        _inventoryItems[slot] = pickup.item;
        _itemPool.Release(pickup.gameObject);
        InventoryEvents.InventoryUpdate();
    }

    public void DropItem(InventorySlot slot)
    {
        GameObject obj = _itemPool.Get();
        AssignItemData(obj, _inventoryItems[slot]);
        Progression.Instance.RemoveStoryValue(_inventoryItems[slot].associatedStoryValue);
        _inventoryItems[slot] = null;
        InventoryEvents.InventoryUpdate();
    }

    public void EquipItem(InventorySlot slot)
    {
        var equipContainer = GetEquipContainer(_inventoryItems[slot].type);
        InventorySlot equipSlot = GetFirstEmptySlot(equipContainer);

        if (equipSlot != null)
        {
            equipContainer[equipSlot] = _inventoryItems[slot];
            _inventoryItems[slot] = null;
            InventoryEvents.InventoryUpdate();
            InventoryEvents.ShowItemDetails(equipSlot);
            if(_firstTimeEquip)
            {
                _firstTimeEquip = false;
                InteractionManager.Instance.SetInfoText("Press [1] to select equipped weapon and [X] to take it out.");
            }
        }
    }

    public void UnequipItem(InventorySlot slot)
    {
        var equipContainer = GetEquipContainer(GetItemFromContainer(slot).type);
        InventorySlot itemSlot = GetFirstEmptySlot(_inventoryItems);

        if (itemSlot != null)
        {
            _inventoryItems[itemSlot] = equipContainer[slot];
            equipContainer[slot] = null;
            InventoryEvents.InventoryUpdate();
            InventoryEvents.ShowItemDetails(itemSlot);
        }
    }

    public void SortItems()
    {
        List<InventorySlot> slots = new();
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

    private void AssignItemData(GameObject obj, Item item)
    {
        /*float maxSize = 3;
        MeshFilter mf = obj.GetComponent<MeshFilter>();
        mf.mesh = item.modelMesh;

        foreach(var bc in obj.GetComponents<BoxCollider>())
        {
            Bounds b = mf.sharedMesh.bounds;
            bc.center = b.center;

            if(bc.isTrigger) // trigger collider should be increased by maxSize to make picking up things easier
            {
                bc.size = b.size + new Vector3(maxSize, maxSize, maxSize);
            }
            else
            {
                bc.size = b.size;
            }
        }*/
        float maxSize = 2;
        MeshFilter mf = obj.GetComponent<MeshFilter>();
        mf.mesh = item.modelMesh;

        BoxCollider bc = obj.GetComponent<BoxCollider>();
        SphereCollider sc = obj.GetComponent<SphereCollider>();
        Bounds b = mf.sharedMesh.bounds;
        bc.center = b.center;
        bc.size = b.size;

        sc.center = b.center;
        sc.radius = Mathf.Max(b.size.x, b.size.y, b.size.z) / 2 + maxSize;

        obj.GetComponent<InteractPickup>().item = item;
        obj.GetComponent<MeshRenderer>().material = item.modelMaterial;
        obj.name = item.itemName;
    }

    public void AddWeaponSlot(InventorySlot slot)
    {
        _equippedWeapons.Add(slot, null);
    }
    
    public void AddMovementItemSlot(InventorySlot slot)
    {
        _equippedMovementItems.Add(slot, null);
    }

    public void AddInventorySlot(InventorySlot slot)
    {
        _inventoryItems.Add(slot, null);
    }

    public Item GetPrimaryWeapon()
    {
        var e = _equippedWeapons.GetEnumerator();
        e.MoveNext();
        return e.Current.Value;
    }

    public Item GetSecondaryWeapon()
    {
        var e = _equippedWeapons.GetEnumerator();
        e.MoveNext();
        e.MoveNext();
        return e.Current.Value;
    }

    public Item GetItemFromContainer(InventorySlot slot)
    {
        if (_equippedWeapons.ContainsKey(slot)) return _equippedWeapons[slot];
        else if (_equippedMovementItems.ContainsKey(slot)) return _equippedMovementItems[slot];
        else return _inventoryItems[slot];
    }

    private Dictionary<InventorySlot, Item> GetEquipContainer(ItemType itemType)
    {
        if (itemType == ItemType.Weapon) return _equippedWeapons;
        if (itemType == ItemType.MovementItem) return _equippedMovementItems;
        return null;
    }

    private InventorySlot GetFirstEmptySlot(Dictionary<InventorySlot, Item> container)
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
        obj.transform.position = _itemDropTransform.position + _itemDropTransform.forward.normalized;
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