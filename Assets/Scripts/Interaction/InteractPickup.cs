using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
        InteractionManager.Instance.InteractionText.SetText($"Press [E] to pick up {item.itemName}");
    }

    public void OnEndLook()
    {
        //interactionUIText.SetText("");
        InteractionManager.Instance.InteractionText.SetText("");
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        Progression.Instance.AddStoryValue(item.associatedStoryValue);
        Inventory.Instance.AddItem(this);
    }
}