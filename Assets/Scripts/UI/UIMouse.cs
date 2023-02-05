using UnityEngine;
using UnityEngine.UI;

public class UIMouse : MonoBehaviour
{
    [SerializeField] private Image cursorImage;
    [SerializeField] private Sprite cursorOpen;
    [SerializeField] private Sprite cursorClose;
    [SerializeField] private ItemMove itemMove;

    private InputManager inputManager;
    private bool cursorClosed = false;

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    void Update()
    {
        transform.position = inputManager.GetCursorPosition();

        if(itemMove.heldItem != null && !cursorClosed)
        {
            cursorClosed = true;
            cursorImage.sprite = cursorClose;
        }
        else if (itemMove.heldItem == null && cursorClosed) 
        {
            cursorClosed = false;
            cursorImage.sprite = cursorOpen;
        }
    }
}
