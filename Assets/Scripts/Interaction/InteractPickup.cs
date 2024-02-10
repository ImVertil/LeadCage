using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Outline))]
public class InteractPickup : MonoBehaviour, IInteractable
{
    public Item item;
    public GameObject interactionTextObject;
    private TMP_Text interactionUIText;
    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void OnStartLook()
    {
        //interactionUIText.SetText($"Pick up {item.itemName}");
        InteractionManager.Instance.SetInteractionText($"Press [E] to pick up {item.itemName}");
        _outline.enabled = true;
    }

    public void OnEndLook()
    {
        //interactionUIText.SetText("");
        InteractionManager.Instance.SetInteractionText("");
        _outline.enabled = false;
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        _outline.enabled = false;
        Progression.Instance.AddStoryValue(item.associatedStoryValue);
        Inventory.Instance.AddItem(this);
    }
}