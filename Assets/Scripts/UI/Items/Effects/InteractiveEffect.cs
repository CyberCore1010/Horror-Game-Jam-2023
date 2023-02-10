using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveEffect : Effect
{
    [SerializeField] private ItemInteractable InteractionTarget;
    [SerializeField] private bool ConsumesItem;

    public override bool Trigger()
    {
        if(InteractionTarget.isAwaiting)
        {
            InventoryHandler.Instance.ForceClose();
            return ConsumesItem;
        }
        else
        {
            PlayerVoiceLineHandler.Instance.CantUseThat();
        }

        return false;
    }
}
