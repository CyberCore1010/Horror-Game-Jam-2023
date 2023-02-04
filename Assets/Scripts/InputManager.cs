using UnityEngine;

public class InputManager : MonoBehaviour
{
    [HideInInspector] public PlayerControls playerControls;

    private static InputManager _instance;

    public static InputManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
        playerControls = new PlayerControls();
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
}
