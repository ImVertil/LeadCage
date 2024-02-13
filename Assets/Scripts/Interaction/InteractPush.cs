using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Outline))]
public class InteractPush : MonoBehaviour, IInteractable
{
    public GameObject interactionTextObject;
    [SerializeField] private float _range = 1000f;
    [SerializeField] private float _impactForce = 100f;
    [SerializeField] private Rigidbody _bodyToPush;
    [SerializeField] private GameObject _player;
    private bool _isNotAChild;
    private Outline _outline;


    public GameObject bulletOrigin;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
        _isNotAChild = true;
    }

    public void OnStartLook()
    {
        InteractionManager.Instance.SetInteractionText($"Press [E] to push object");
        _outline.enabled = true;
    }

    public void OnEndLook()
    {
        InteractionManager.Instance.SetInteractionText("");
        _outline.enabled = false;
    }
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        _outline.enabled = false;
        RaycastHit hit;
        if (Physics.Raycast(bulletOrigin.transform.position, bulletOrigin.transform.forward, out hit, _range))
        {
            if (hit.rigidbody == _bodyToPush)
            {
                hit.rigidbody.AddForce(-hit.normal * _impactForce);
            }
        }
    }
}
