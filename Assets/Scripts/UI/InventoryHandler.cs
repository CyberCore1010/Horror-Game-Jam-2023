using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryHandler : MonoBehaviour
{
    public List<InventoryTile> InventoryTiles;

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

    public InventoryTile GetAdjacentTile(InventoryTile inventoryTile, TileOrientation tileOrientation)
    {
        int currentIndex = InventoryTiles.IndexOf(inventoryTile);

        if(tileOrientation == TileOrientation.Horizontal)
        {
            if(! new[] { 3, 7, 11, 15, 19 }.Contains(currentIndex))
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
        return false;
    }
}
