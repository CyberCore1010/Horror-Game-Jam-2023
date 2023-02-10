using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField] private Item item;

    private float timer = -1;

    public void StartInteraction()
    {
        timer = 0;
    }

    public void StopInteraction()
    {
        if(timer < 0.2f)
        {
            if(InventoryHandler.Instance.AssignFreeSlot(item))
            {
                Destroy(gameObject);
            }
            else
            {
                //"I don't have room for this"
            }
        }
        timer = -1;
    }

    private void FixedUpdate()
    {
        if(timer != -1)
        {
            timer += Time.deltaTime;
        }
    }
}
