using TMPro;

public class StackableItem : Item
{
    public int currentStack;
    public int maxStack;
    public int minStack;
    public TextMeshProUGUI itemNumberText;

    private bool itemMaxedOut = false;
    private bool itemMinedOut = false;

    public override void SpecificUpdate()
    {
        if(itemNumberText.text != currentStack.ToString())
        {
            itemNumberText.text = currentStack.ToString();
        }

        if (currentStack == maxStack && !itemMaxedOut)
        {
            itemMaxedOut = true;
            itemMinedOut = false;
            itemNumberText.color = new UnityEngine.Color32(50, 205, 50, 255);
        }
        else if (currentStack <= minStack && !itemMinedOut) {
            itemMaxedOut = false;
            itemMinedOut = true;
            itemNumberText.color = new UnityEngine.Color32(210, 43, 43, 255);
        }
        else if (currentStack > minStack && currentStack != maxStack && (itemMaxedOut || itemMinedOut))
        {
            itemMaxedOut = false;
            itemMinedOut = false;
            itemNumberText.color = UnityEngine.Color.white;
        }
    }
}
