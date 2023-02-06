using UnityEngine;

public class DoubleItem : Item
{
    public InventoryHandler.TileOrientation orientation = InventoryHandler.TileOrientation.Horizontal;

    private void Start()
    {
        itemImage.sprite = ItemDetails.Instance.GetByID(ID).icon;
        InventoryHandler.Instance.GetAdjacentTile(transform.parent.GetComponentInParent<InventoryTile>(), orientation).SlotBlockedBy = this;
    }
}
