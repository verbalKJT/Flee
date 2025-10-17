using UnityEngine;

public class InteractableToothbrush : MonoBehaviour, IInteractable
{
    public string GetPromptText()
    {
        return "[E] 칫솔 줍기";
    }

    public void Interact()
    {
        PlayerInventory.Instance.PickupToothbrush(gameObject);
    }
}
