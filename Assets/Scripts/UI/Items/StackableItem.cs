using TMPro;

public class StackableItem : Item
{
    public int currentStack;
    public int maxStack;
    public int minStack;
    public TextMeshProUGUI itemNumberText;

    public override void SpecificUpdate()
    {
        if(itemNumberText.text != currentStack.ToString())
        {
            itemNumberText.text = currentStack.ToString();
        }
    }
}
