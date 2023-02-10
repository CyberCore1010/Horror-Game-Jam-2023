using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;

    private static PlayerStats _instance;

    public static PlayerStats Instance
    {
        get { return _instance; }
    }

    private void Start()
    {
        _instance = this;
    }
}
