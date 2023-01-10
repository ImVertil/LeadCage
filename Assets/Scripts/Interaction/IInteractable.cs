public interface IInteractable
{
    float MaxRange { get; }

    void OnStartLook();
    void OnInteract();
    void OnEndLook();
}