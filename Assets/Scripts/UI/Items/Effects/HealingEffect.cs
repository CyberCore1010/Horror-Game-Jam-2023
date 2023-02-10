using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEffect : Effect
{
    [SerializeField] private int healingAmount;

    public override bool Trigger()
    {
        PlayerStats.Instance.health += healingAmount;
        if (PlayerStats.Instance.health > PlayerStats.Instance.maxHealth) PlayerStats.Instance.health = PlayerStats.Instance.maxHealth;
        return true;
    }
}
