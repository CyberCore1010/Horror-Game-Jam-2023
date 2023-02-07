using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemDetails.ItemID ID;
    public Image ItemImage;

    private void Start()
    {
        ItemImage.sprite = ItemDetails.Instance.GetByID(ID).icon;
    }

    public void Update()
    {
        SpecificUpdate();
    }

    public virtual void SpecificUpdate() { }
}
