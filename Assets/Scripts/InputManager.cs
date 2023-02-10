using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [HideInInspector] public PlayerControls playerControls;
    [HideInInspector] public PlayerInput inputSystem;

    private static InputManager _instance;
    private Vector2 cursorPosition = Vector2.zero;

    public static InputManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
        playerControls = new PlayerControls();
        SwitchInputSystem(InputMap.Default);
        inputSystem = GetComponent<PlayerInput>();
    }

    public Vector2 GetMovement()
    {
        return playerControls.Default.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return playerControls.Default.Look.ReadValue<Vector2>();
    }

    public Vector2 GetCursorPosition()
    {
        if (inputSystem.currentControlScheme.Equals("Keyboard+Mouse"))
        {
            cursorPosition = Mouse.current.position.ReadValue();
        }
        return cursorPosition;
    }

    public void SwitchInputSystem(InputMap inputMap)
    {
        switch(inputMap)
        {
            case InputMap.Default:
                playerControls.Default.Enable();
                playerControls.UI.Disable();
                break;
            case InputMap.Inventory:
                playerControls.Default.Disable();
                playerControls.UI.Enable();
                ButtonTipHandler.Instance.InventoryTips();
                break;
            case InputMap.Item:
                break;
        }
    }

    public enum InputMap
    {
        Default,
        Inventory,
        Item
    }
}
