using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item ItemInSlot;
    public Item SlotBlockedBy;
    public GameObject ItemContainer;

    [SerializeField] private GameObject HoverGlow;

    private ItemMove itemMove;
    private InputManager inputManager;
    private bool isHovering;

    private void Start()
    {
        itemMove = transform.parent.parent.gameObject.GetComponentInChildren<ItemMove>();

        inputManager = InputManager.Instance;

        inputManager.playerControls.UI.Move.performed += context =>
        {
            if(isHovering)
            {
                itemMove.HandleMovement(GetComponentInChildren<Item>(), ItemContainer.transform, true);
            }
        };

        inputManager.playerControls.UI.Interact.performed += context =>
        {
            if (isHovering)
            {
                itemMove.HandleMovement(GetComponentInChildren<Item>(), ItemContainer.transform, false);
            }
        };

        ItemInSlot = ItemContainer.GetComponentInChildren<Item>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        HoverGlow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        HoverGlow.SetActive(false);
    }
}
