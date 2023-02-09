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
        playerControls.Default.Enable();
        playerControls.UI.Disable();
        inputSystem = GetComponent<PlayerInput>();
    }

    public void OnEnable()
    {
        playerControls.Enable();
    }

    public void OnDisable()
    {
        playerControls.Disable();
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
}
