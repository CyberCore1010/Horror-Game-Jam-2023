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
            if (isHovering)
            {
                if(ItemBlockedBy == null)
                {
                    itemMove.HandleMovement(ItemInSlot, ItemContainer.transform, true);
                }
                else
                {
                    ItemBlockedBy.transform.parent.GetComponentInParent<InventoryTile>().StopHovering();
                    StartHovering();
                    itemMove.HandleMovement(ItemBlockedBy, ItemBlockedBy.transform.parent.transform, true);
                }
            }
        };

        inputManager.playerControls.UI.Interact.performed += context =>
        {
            if (isHovering)
            {
                if (ItemBlockedBy == null)
                {
                    itemMove.HandleMovement(ItemInSlot, ItemContainer.transform, false);
                }
                else
                {
                    ItemBlockedBy.transform.parent.GetComponentInParent<InventoryTile>().StopHovering();
                    StartHovering();
                    itemMove.HandleMovement(ItemBlockedBy, ItemBlockedBy.transform.parent.transform, false);
                }
            }
        };

        inputManager.playerControls.UI.Inventory.performed += context =>
        {
            SingleHoverGlow.SetActive(false);
            DoubleHoverGlow.SetActive(false);
        };

        ItemInSlot = ItemContainer.GetComponentInChildren<Item>();
        DoubleItem doubleInSlot = ItemInSlot as DoubleItem;
        if (doubleInSlot != null)
        {
            InventoryHandler.Instance.GetAdjacentTile(this, doubleInSlot.orientation).ItemBlockedBy = ItemInSlot;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        StartHovering();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopHovering();
    }

    public void StartHovering() {
        isHovering = true;
        if (ItemBlockedBy == null)
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

        if(ItemInSlot != null)
        {
            ItemDetails.ItemDetail itemDetail = ItemDetails.Instance.GetByID(ItemInSlot.ID);
            ItemDescription.Instance.SetItem(itemDetail.name, itemDetail.category.ToString(), itemDetail.description);
        }
        else if(ItemBlockedBy != null)
        {
            ItemDetails.ItemDetail itemDetail = ItemDetails.Instance.GetByID(ItemBlockedBy.ID);
            ItemDescription.Instance.SetItem(itemDetail.name, itemDetail.category.ToString(), itemDetail.description);
        }
    }

    public void StopHovering()
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
        ItemDescription.Instance.Hide();
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
