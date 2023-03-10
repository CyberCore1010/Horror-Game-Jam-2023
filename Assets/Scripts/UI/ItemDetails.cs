using System;
using System.Collections.Generic;
using UnityEngine;


public class ItemDetails : MonoBehaviour
{
    public enum ItemID
    {
        Stackable_Bandage,
        Stackable_Ammo,
        Key,
        Handgun,
        Double_LeadPipe
    }

    public enum ItemCategory
    {
        Healing,
        Ammo,
        Misc,
        Pistol,
        Melee
    }

    [Serializable]
    public struct ItemDetail
    {
        public ItemID ID;
        public ItemCategory category;
        public Sprite icon;
        public string name;
        public string description;
    }

    [SerializeField]
    public List<ItemDetail> items;

    private static ItemDetails _instance;

    public static ItemDetails Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }

    public ItemDetail GetByID(ItemID ID)
    {
        return items.Find(i => i.ID == ID);
    }
}
