using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryHandler : MonoBehaviour
{
    public List<InventoryTile> InventoryTiles;
    public bool OpenForInteraction;

    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private ButtonTipHandler buttonTips;

    public enum TileOrientation
    {
        Vertical,
        Horizontal
    }

    private static InventoryHandler _instance;

    public static InventoryHandler Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void OpenInventoryForInteraction()
    {
        OpenForInteraction = true;
        InputManager.Instance.SwitchInputSystem(InputManager.InputMap.Inventory);
        InventoryUI.SetActive(true);
        buttonTips.InteractionTips();
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Inventory Opened For Interaction");
    }

    public void ForceClose()
    {
        OpenForInteraction = false;
        InputManager.Instance.SwitchInputSystem(InputManager.InputMap.Default);
        InventoryUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Inventory Closed");
    }

    public InventoryTile GetAdjacentTile(InventoryTile inventoryTile, TileOrientation tileOrientation)
    {
        int currentIndex = InventoryTiles.IndexOf(inventoryTile);

        if(tileOrientation == TileOrientation.Horizontal)
        {
            if (! new[] { 3, 7, 11, 15, 19 }.Contains(currentIndex))
            {
                return InventoryTiles[currentIndex + 1];
            }
        }
        else if(tileOrientation == TileOrientation.Vertical)
        {
            if (! new[] { 16, 17, 18, 19 }.Contains(currentIndex))
            {
                return InventoryTiles[currentIndex + 4];
            }
        }

        return null;
    }

    public bool AssignFreeSlot(Item item)
    {
        StackableItem stackableItem = item as StackableItem;
        if(stackableItem != null)
        {
            foreach (InventoryTile inventoryTile in InventoryTiles)
            {
                StackableItem foundStackable = inventoryTile.ItemInSlot as StackableItem;
                if(foundStackable != null && foundStackable.ID == stackableItem.ID && foundStackable.currentStack < foundStackable.maxStack)
                {
                    foundStackable.currentStack += stackableItem.currentStack;
                    if(foundStackable.currentStack <= foundStackable.maxStack)
                    {
                        return true;
                    }
                    else
                    {
                        stackableItem.currentStack = foundStackable.maxStack - foundStackable.currentStack;
                        foundStackable.currentStack = foundStackable.maxStack;
                    }
                }
            }
        }

        int freeTiles = 0;
        DoubleItem doubleItem = item as DoubleItem;

        foreach (InventoryTile inventoryTile in InventoryTiles)
        {
            if (doubleItem != null)
            {
                if (inventoryTile.ItemInSlot == null && inventoryTile.ItemBlockedBy == null)
                {
                    freeTiles++;
                    InventoryTile adjacentTile = GetAdjacentTile(inventoryTile, doubleItem.orientation);
                    if (adjacentTile != null && adjacentTile.ItemInSlot == null)
                    {
                        doubleItem.transform.SetParent(inventoryTile.ItemContainer.transform);
                        doubleItem.transform.localPosition = Vector3.zero;
                        inventoryTile.ItemInSlot = doubleItem;

                        adjacentTile.ItemInSlot = null;
                        adjacentTile.ItemBlockedBy = doubleItem;

                        return true;
                    }
                }
            }
            else
            {
                if(inventoryTile.ItemInSlot == null && inventoryTile.ItemBlockedBy == null)
                {
                    item.transform.SetParent(inventoryTile.ItemContainer.transform);
                    item.transform.localPosition = Vector3.zero;
                    inventoryTile.ItemInSlot = item;

                    return true;
                }
            }
        }

        if(freeTiles >= 2)
        {
            for (int i = InventoryTiles.Count-2; i < freeTiles; i--)
            {
                InventoryTile adjacentTile = GetAdjacentTile(InventoryTiles[i], doubleItem.orientation);
                if (!(InventoryTiles[i].ItemInSlot as DoubleItem) && !(adjacentTile.ItemInSlot as DoubleItem))
                {
                    Item item1 = InventoryTiles[i].ItemInSlot;
                    Item item2 = adjacentTile.ItemInSlot;

                    doubleItem.transform.SetParent(InventoryTiles[i].ItemContainer.transform);
                    doubleItem.transform.localPosition = Vector3.zero;
                    InventoryTiles[i].ItemInSlot = doubleItem;

                    adjacentTile.ItemInSlot = null;
                    adjacentTile.ItemBlockedBy = doubleItem;

                    AssignFreeSlot(item1);
                    AssignFreeSlot(item2);

                    return true;
                }
            }
        }

        return false;
    }
}
