using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemActions : MonoBehaviour
{
    Item item;
    public Button RemoveButton;

    public void DeleteItem()
    {
        Inventory.Instance.Remove(item);
        Destroy(gameObject);
    }
    public void AddItem(Item newItem)
    {
        item = newItem;
    }

}
