using TMPro;
using UnityEngine;

public class InteractPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string _itemName;
    public GameObject interactionTextObject;
    private TMP_Text interactionUIText;
    private Item _item;

    private void Awake()
    {
        interactionUIText = interactionTextObject.GetComponent<TMP_Text>();
        if(_itemName == "")
        {
            _itemName = "undefined";
        }
        _item = GetComponent<ItemController>().Item;
    }

    public void OnStartLook()
    {
        interactionUIText.SetText($"Pick up {_itemName}");
    }

    public void OnEndLook()
    {
        interactionUIText.SetText("");
    }

    public void OnInteract()
    {
        Destroy(this.gameObject);
        Progression.Instance.AddStoryValue(StoryValue.HasScrewdriver);
        Inventory.Instance.Add(_item);
    }
}