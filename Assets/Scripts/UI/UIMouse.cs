using UnityEngine;
using UnityEngine.UI;

public class UIMouse : MonoBehaviour
{
    [SerializeField] private Image cursorImage;
    [SerializeField] private Sprite cursorOpen;
    [SerializeField] private Sprite cursorClose;
    [SerializeField] private ItemMove itemMove;

    [SerializeField] private CursorSnapPoint defaultSnapPoint;

    private Vector2 cursorPosition;
    private CursorSnapPoint currentSnapPoint;

    private InputManager inputManager;
    private bool cursorClosed = false;

    private void Start()
    {
        inputManager = InputManager.Instance;

        inputManager.playerControls.UI.ButtonSnapUp.performed += context =>
        {
            if(currentSnapPoint == null)
            {
                currentSnapPoint = defaultSnapPoint;
                currentSnapPoint.ConnectedTile.StartHovering();
            }
            else
            {
                currentSnapPoint.ConnectedTile.StopHovering();
                currentSnapPoint = currentSnapPoint.UpSnap;
                currentSnapPoint.ConnectedTile.StartHovering();
            }
            transform.position = currentSnapPoint.transform.position;
        };

        inputManager.playerControls.UI.ButtonSnapDown.performed += context =>
        {
            if (currentSnapPoint == null)
            {
                currentSnapPoint = defaultSnapPoint;
                currentSnapPoint.ConnectedTile.StartHovering();
            }
            else
            {
                currentSnapPoint.ConnectedTile.StopHovering();
                currentSnapPoint = currentSnapPoint.DownSnap;
                currentSnapPoint.ConnectedTile.StartHovering();
            }
            transform.position = currentSnapPoint.transform.position;
        };

        inputManager.playerControls.UI.ButtonSnapLeft.performed += context =>
        {
            if (currentSnapPoint == null)
            {
                currentSnapPoint = defaultSnapPoint;
                currentSnapPoint.ConnectedTile.StartHovering();
            }
            else
            {
                currentSnapPoint.ConnectedTile.StopHovering();
                currentSnapPoint = currentSnapPoint.LeftSnap;
                currentSnapPoint.ConnectedTile.StartHovering();
            }
            transform.position = currentSnapPoint.transform.position;
        };

        inputManager.playerControls.UI.ButtonSnapRight.performed += context =>
        {
            if (currentSnapPoint == null)
            {
                currentSnapPoint = defaultSnapPoint;
                currentSnapPoint.ConnectedTile.StartHovering();
            }
            else
            {
                currentSnapPoint.ConnectedTile.StopHovering();
                currentSnapPoint = currentSnapPoint.RightSnap;
                currentSnapPoint.ConnectedTile.StartHovering();
            }
            transform.position = currentSnapPoint.transform.position;
        };
    }

    void Update()
    {
        Vector2 tempPosition = inputManager.GetCursorPosition();
        if (cursorPosition != tempPosition)
        {
            cursorPosition = tempPosition;
            transform.position = cursorPosition;

            if (itemMove.heldItem != null && !cursorClosed)
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
}
