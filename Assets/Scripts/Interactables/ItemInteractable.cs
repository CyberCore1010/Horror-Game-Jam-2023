
public class ItemInteractable : Interactable
{
    public bool isAwaiting;

    private void Start()
    {
        InputManager.Instance.playerControls.UI.Inventory.performed += context =>
        {
            isAwaiting = false;
        };
    }

    public void StartInteraction()
    {
        isAwaiting = true;
        InventoryHandler.Instance.OpenInventoryForInteraction();
    }
}
