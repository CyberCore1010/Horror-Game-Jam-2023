using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item ItemInSlot;
    public Item ItemBlockedBy;
    public GameObject ItemContainer;
    public GameObject SingleHoverGlow;
    public GameObject DoubleHoverGlow;

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

        inputManager.playerControls.UI.Inventory.performed += context =>
        {
            SingleHoverGlow.SetActive(false);
            DoubleHoverGlow.SetActive(false);
        };

        ItemInSlot = ItemContainer.GetComponentInChildren<Item>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        if(ItemBlockedBy == null)
        {
            if (ItemInSlot as DoubleItem)
            {
                DoubleHoverGlow.SetActive(true);
            }
            else
            {
                SingleHoverGlow.SetActive(true);
            }
        } 
        else
        {
            InventoryTile blockedByTile = ItemBlockedBy.transform.parent.GetComponentInParent<InventoryTile>();

            if (ItemBlockedBy as DoubleItem)
            {
                blockedByTile.DoubleHoverGlow.SetActive(true);
            }
            else
            {
                blockedByTile.SingleHoverGlow.SetActive(true);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if (ItemBlockedBy == null)
        {
            SingleHoverGlow.SetActive(false);
            DoubleHoverGlow.SetActive(false);
        }
        else
        {
            InventoryTile blockedByTile = ItemBlockedBy.transform.parent.GetComponentInParent<InventoryTile>();
            blockedByTile.SingleHoverGlow.SetActive(false);
            blockedByTile.DoubleHoverGlow.SetActive(false);
        }
    }

    public void SwitchToDoubleGlow()
    {
        SingleHoverGlow.SetActive(false);
        DoubleHoverGlow.SetActive(true);
    }

    public void SwitchToSingleGlow()
    {
        SingleHoverGlow.SetActive(true);
        DoubleHoverGlow.SetActive(false);
    }
}
