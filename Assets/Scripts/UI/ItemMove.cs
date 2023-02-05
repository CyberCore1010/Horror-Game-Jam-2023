using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemMove : MonoBehaviour
{
    [SerializeField] public Item heldItem;

    public void HandleMovement(Item item, Transform itemContainer, bool moveSpecificallyUsed)
    {
        if (item != null && heldItem == null && moveSpecificallyUsed)
        {
            heldItem = item;
            heldItem.transform.SetParent(transform);
            heldItem.transform.localPosition = Vector3.zero;
        }
        else if (item == null && heldItem != null)
        {
            heldItem.transform.SetParent(itemContainer);
            heldItem.transform.localPosition = Vector3.zero;
            heldItem = null;
        }
        else if (item != null && heldItem != null)
        {
            StackableItem heldStackable = heldItem as StackableItem;
            StackableItem stackable = item as StackableItem;
            if(heldStackable != null && stackable != null && stackable.ID == heldStackable.ID && stackable.currentStack < stackable.maxStack)
            {
                stackable.currentStack += heldStackable.currentStack;
                heldStackable.currentStack = 0;
                if(stackable.currentStack > stackable.maxStack)
                {
                    heldStackable.currentStack = stackable.currentStack - stackable.maxStack;
                    stackable.currentStack = stackable.maxStack;
                }

                if (heldStackable.currentStack <= 0)
                {
                    Destroy(heldItem.transform.gameObject);
                }
            }
            else
            {
                heldItem.transform.SetParent(itemContainer);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem = item;
                heldItem.transform.SetParent(transform);
                heldItem.transform.localPosition = Vector3.zero;
            }
        }
    }
}
