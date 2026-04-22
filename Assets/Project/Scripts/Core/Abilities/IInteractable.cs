public interface IInteractable
{
<<<<<<< HEAD
    void Interact();
=======
    void Interact(InteractionContext context);
>>>>>>> 7ca46c4 (restore scripts interaction system)
    string GetInteractionText();
}