using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemDetails.ItemID ID;
    public Image itemImage;

    private void Start()
    {
        itemImage.sprite = ItemDetails.Instance.GetByID(ID).icon;
    }

    public void Update()
    {
        SpecificUpdate();    
    }

    public virtual void SpecificUpdate() { }
}
