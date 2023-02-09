using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTipHandler : MonoBehaviour
{
    [SerializeField] private ItemMove itemMove;
    [SerializeField] private GameObject InventoryButtonTips;
    [SerializeField] private GameObject HoldingButtonTips;

    private void Update()
    {
        if(itemMove.heldItem == null) {
            InventoryButtonTips.SetActive(true);
            HoldingButtonTips.SetActive(false);
        }
        else if(itemMove.heldItem != null)
        {
            InventoryButtonTips.SetActive(false);
            HoldingButtonTips.SetActive(true);
        }
    }
}
