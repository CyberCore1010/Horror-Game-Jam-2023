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
            if(tile.ItemBlockedBy != null)
            {
                heldItem.transform.SetParent(itemContainer);
                heldItem.transform.localPosition = Vector3.zero;

                tile.ItemBlockedBy.transform.SetParent(transform);
                tile.ItemBlockedBy.transform.localPosition = Vector3.zero;

                heldItem = tile.ItemBlockedBy;
                tile.ItemBlockedBy = null;
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
                InventoryHandler.Instance.GetAdjacentTile(tile, doubleItem.orientation).ItemBlockedBy = null;
            }
            
            previousItemContainer = itemContainer;
            previousInventoryTile = tile;
        }
        else if (item == null && heldItem != null)
        {
            DoubleItem heldDoubleItem = heldItem as DoubleItem;
            if (heldDoubleItem != null)
            {
                InventoryTile adjacentTile = InventoryHandler.Instance.GetAdjacentTile(tile, heldDoubleItem.orientation);
                if (adjacentTile == null)
                    return;

                Item secondItem = adjacentTile.ItemInSlot;

                adjacentTile.ItemInSlot = null;
                adjacentTile.ItemBlockedBy = heldDoubleItem;

                if (secondItem != null)
                {
                    previousInventoryTile.ItemInSlot = secondItem;
                    secondItem.transform.SetParent(previousItemContainer);
                    secondItem.transform.localPosition = Vector3.zero;
                }

                tile.SwitchToDoubleGlow();
            }

            if (tile.ItemBlockedBy == null)
            {
                heldItem.transform.SetParent(itemContainer);
                heldItem.transform.localPosition = Vector3.zero;
                tile.ItemInSlot = heldItem;
                heldItem = null;
            }
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
                    InventoryHandler.Instance.GetAdjacentTile(tile, doubleItem.orientation).ItemBlockedBy = heldItem;
                    heldItem.transform.SetParent(itemContainer);
                    heldItem.transform.localPosition = Vector3.zero;
                    tile.ItemInSlot = heldItem;

                    InventoryHandler.Instance.GetAdjacentTile(previousInventoryTile, doubleItem.orientation).ItemBlockedBy = item;
                    item.transform.SetParent(previousItemContainer);
                    item.transform.localPosition = Vector3.zero;
                    previousInventoryTile.ItemInSlot = heldItem;

                    heldItem = null;
                }
                else
                {
                    InventoryTile adjacentTile = InventoryHandler.Instance.GetAdjacentTile(tile, heldDoubleItem.orientation);
                    
                    if (adjacentTile == null)
                        return;

                    Item secondItem = adjacentTile.ItemInSlot;

                    adjacentTile.ItemInSlot = null;
                    adjacentTile.ItemBlockedBy = heldDoubleItem;
                    tile.ItemInSlot = heldDoubleItem;
                    heldItem.transform.SetParent(itemContainer);
                    heldItem.transform.localPosition = Vector3.zero;
                    
                    if(adjacentTile != previousInventoryTile)
                    {
                        previousInventoryTile.ItemInSlot = item;
                        item.transform.SetParent(previousItemContainer);
                        item.transform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        InventoryTile ohShitTile = InventoryHandler.Instance.GetAdjacentTile(adjacentTile, heldDoubleItem.orientation);

                        ohShitTile.ItemInSlot = item;
                        item.transform.SetParent(ohShitTile.ItemContainer.transform);
                        item.transform.localPosition = Vector3.zero;
                    }

                    if (secondItem != null)
                    {
                        InventoryTile previousAdjacentTile = InventoryHandler.Instance.GetAdjacentTile(previousInventoryTile, heldDoubleItem.orientation);
                        previousAdjacentTile.ItemBlockedBy = null;

                        previousAdjacentTile.ItemInSlot = secondItem;
                        secondItem.transform.SetParent(previousAdjacentTile.ItemContainer.transform);
                        secondItem.transform.localPosition = Vector3.zero;
                    }

                    heldItem = null;
                    tile.SwitchToDoubleGlow();
                }
            }
            else
            {
                if (doubleItem != null)
                {
                    InventoryTile adjacentTile = InventoryHandler.Instance.GetAdjacentTile(tile, doubleItem.orientation);
                    InventoryTile previousAdjacentTile = InventoryHandler.Instance.GetAdjacentTile(previousInventoryTile, doubleItem.orientation);
                    adjacentTile.ItemBlockedBy = null;
                    adjacentTile.ItemInSlot = previousAdjacentTile.ItemInSlot;
                    adjacentTile.ItemInSlot.transform.SetParent(adjacentTile.ItemContainer.transform);
                    adjacentTile.ItemInSlot.transform.localPosition = Vector3.zero;
                    previousAdjacentTile.ItemInSlot = null;
                    previousAdjacentTile.ItemBlockedBy = doubleItem;
                }

                heldItem.transform.SetParent(itemContainer);
                heldItem.transform.localPosition = Vector3.zero;
                tile.ItemInSlot = heldItem;

                item.transform.SetParent(previousItemContainer);
                item.transform.localPosition = Vector3.zero;
                previousInventoryTile.ItemInSlot = item;

                heldItem = null;
                tile.SwitchToSingleGlow();
            }
        }
    }
}
