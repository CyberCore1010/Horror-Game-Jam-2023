using System;
using UnityEngine;

public class ItemMove : MonoBehaviour
{
    public Item heldItem;
    
    private Transform previousItemContainer;
    private InventoryTile previousInventoryTile; 

    public void HandleMovement(Item item, Transform itemContainer, bool moveSpecificallyUsed)
    {
        InventoryTile tile = itemContainer.parent.GetComponent<InventoryTile>();
        
        if (item != null && heldItem == null && moveSpecificallyUsed)
        {
            if(tile.SlotBlockedBy != null)
            {
                heldItem.transform.SetParent(itemContainer);
                heldItem.transform.localPosition = Vector3.zero;

                tile.SlotBlockedBy.transform.SetParent(transform);
                tile.SlotBlockedBy.transform.localPosition = Vector3.zero;

                heldItem = tile.SlotBlockedBy;
                tile.SlotBlockedBy = null;
            }
            else
            {
                heldItem = item;
                heldItem.transform.SetParent(transform);
                heldItem.transform.localPosition = Vector3.zero;
                tile.ItemInSlot = null;
            }
            
            DoubleItem doubleItem = item as DoubleItem;
            if(doubleItem != null)
            {
                InventoryHandler.Instance.GetAdjacentTile(tile, doubleItem.orientation).SlotBlockedBy = null;
            }
            
            previousItemContainer = itemContainer;
            previousInventoryTile = tile;
        }
        else if (item == null && heldItem != null)
        {
            heldItem.transform.SetParent(itemContainer);
            heldItem.transform.localPosition = Vector3.zero;
            tile.ItemInSlot = heldItem;
            heldItem = null;
        }
        else if (item != null && heldItem != null)
        {
            StackableItem heldStackable = heldItem as StackableItem;
            StackableItem stackable = item as StackableItem;

            DoubleItem heldDoubleItem = heldItem as DoubleItem;
            DoubleItem doubleItem = item as DoubleItem;

            if (heldStackable != null && stackable != null && stackable.ID == heldStackable.ID && stackable.currentStack < stackable.maxStack)
            {
                stackable.currentStack += heldStackable.currentStack;
                heldStackable.currentStack = 0;
                if (stackable.currentStack > stackable.maxStack)
                {
                    heldStackable.currentStack = stackable.currentStack - stackable.maxStack;
                    stackable.currentStack = stackable.maxStack;
                }

                if (heldStackable.currentStack <= 0)
                {
                    Destroy(heldItem.transform.gameObject);
                }
            }
            else if (heldDoubleItem != null)
            {
                if (doubleItem != null && heldDoubleItem.orientation == doubleItem.orientation)
                {
                    InventoryHandler.Instance.GetAdjacentTile(tile, doubleItem.orientation).SlotBlockedBy = heldItem;
                    heldItem.transform.SetParent(itemContainer);
                    heldItem.transform.localPosition = Vector3.zero;
                    tile.ItemInSlot = heldItem;

                    InventoryHandler.Instance.GetAdjacentTile(previousInventoryTile, doubleItem.orientation).SlotBlockedBy = item;
                    item.transform.SetParent(previousItemContainer);
                    item.transform.localPosition = Vector3.zero;
                    previousInventoryTile.ItemInSlot = heldItem;

                    heldItem = null;
                }
                else
                {
                    InventoryTile adjacentTile = InventoryHandler.Instance.GetAdjacentTile(tile, doubleItem.orientation);
                    Item secondItem = adjacentTile.ItemInSlot;

                    adjacentTile.ItemInSlot = null;
                    adjacentTile.SlotBlockedBy = heldDoubleItem;
                    tile.ItemInSlot = heldDoubleItem;
                    heldItem.transform.SetParent(itemContainer);
                    heldItem.transform.localPosition = Vector3.zero;
                    
                    previousInventoryTile.ItemInSlot = item;
                    item.transform.SetParent(previousItemContainer);
                    item.transform.localPosition = Vector3.zero;

                    InventoryTile previousAdjacentTile = InventoryHandler.Instance.GetAdjacentTile(previousInventoryTile, heldDoubleItem.orientation);
                    previousAdjacentTile.SlotBlockedBy = null;

                    if (secondItem != null)
                    {
                        previousAdjacentTile.ItemInSlot = secondItem;
                        secondItem.transform.SetParent(previousAdjacentTile.ItemContainer.transform);
                        secondItem.transform.localPosition = Vector3.zero;
                    }

                    heldItem = null;
                }
            }
            else
            {
                heldItem.transform.SetParent(itemContainer);
                heldItem.transform.localPosition = Vector3.zero;
                tile.ItemInSlot = heldItem;

                item.transform.SetParent(previousItemContainer);
                item.transform.localPosition = Vector3.zero;
                previousInventoryTile.ItemInSlot = item;

                heldItem = null;
            }
        }
    }
}
