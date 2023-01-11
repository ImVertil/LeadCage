using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public Transform InventoryBox;
    public GameObject InventoryCanvas;
    public GameObject InventoryItem;
    public ItemActions[] InventoryItems;
    public List<Item> Items = new List<Item>();
    private bool _isVisible;
    private void Awake()
    {
        Instance = this;
        _isVisible = false;
}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleVisibility();
        }
    }
    public void Add(Item item)
    {
        Items.Add(item);
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    public void ListItems()
    {

        foreach(Transform item in InventoryBox)
        {
            Destroy(item.gameObject);
        }
        foreach(var item in Items)
        {
            GameObject gameObject = Instantiate(InventoryItem, InventoryBox);
            var itemName = gameObject.transform.Find("ItemName").GetComponent<Text>();
            var itemIcon = gameObject.transform.Find("ItemIcon").GetComponent<Image>();
            itemName.text = item.ItemName;
            itemIcon.sprite = item.Icon;
        }
        SetItems();
    }

    public void SetItems()
    {
        InventoryItems = InventoryBox.GetComponentsInChildren<ItemActions>();
        for (int i = 0; i < Items.Count; i++)
        {
            InventoryItems[i].AddItem(Items[i]);
        }
    }

    private void ToggleVisibility()
    {
        _isVisible = !_isVisible;
        InventoryCanvas.gameObject.SetActive(_isVisible);
        ListItems();
    }
}
