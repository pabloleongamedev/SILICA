public interface IInteractable
{
    void Interact(InteractionContext context);
    string GetInteractionText();
}