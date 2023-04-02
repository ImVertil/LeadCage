using TMPro;
using UnityEngine;

public class InteractPickup : MonoBehaviour, IInteractable
{
    public Item item;
    public GameObject interactionTextObject;
    private TMP_Text interactionUIText;

    private void Awake()
    {
        //interactionUIText = interactionTextObject.GetComponent<TMP_Text>();
    }

    public void OnStartLook()
    {
        //interactionUIText.SetText($"Pick up {item.itemName}");
    }

    public void OnEndLook()
    {
        //interactionUIText.SetText("");
    }

    public void OnInteract()
    {
        Progression.Instance.AddStoryValue(item.associatedStoryValue);
        //Inventory.Instance.AddItem(this);
        //InventoryNew.Instance.Test(this.gameObject);
    }
}