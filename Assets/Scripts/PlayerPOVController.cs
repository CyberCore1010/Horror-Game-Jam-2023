using UnityEngine;
using Cinemachine;

public class PlayerPOVController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 50f;
    [SerializeField] private float verticalSpeed = 50f;
    [SerializeField] private float catchupModifier = 0.0015f;
    [SerializeField] private float clampAngle = 80f;

    [SerializeField] private Transform flashlightAim;
    [SerializeField] private Transform head;
    [SerializeField] private Flashlight flashlight;

    private Vector3 flashlightRotation;
    private Vector3 BodyRotation;
    private Vector3 HeadRotation;

    private InputManager inputManager;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void FixedUpdate()
    {
        if (flashlightRotation == null) flashlightRotation = flashlightAim.rotation.eulerAngles;
        if (BodyRotation == null) HeadRotation = transform.rotation.eulerAngles;
        if (HeadRotation == null) HeadRotation = head.localRotation.eulerAngles;

        Cursor.lockState = CursorLockMode.Locked;

        Vector2 deltaInput = inputManager.GetMouseDelta();
        flashlightRotation.y += deltaInput.x * verticalSpeed * Time.deltaTime;
        flashlightRotation.x += deltaInput.y * horizontalSpeed * Time.deltaTime;
        flashlightRotation.x = Mathf.Clamp(flashlightRotation.x, -clampAngle, clampAngle);
        flashlightAim.rotation = Quaternion.Euler(-flashlightRotation.x, flashlightRotation.y, 0f);

        flashlight.PointAt(flashlightAim.transform.forward * 100);

        BodyRotation.Set(BodyRotation.x, Mathf.Lerp(BodyRotation.y, flashlightRotation.y, horizontalSpeed * catchupModifier), BodyRotation.z);
        transform.rotation = Quaternion.Euler(BodyRotation.x, BodyRotation.y, BodyRotation.z);
        HeadRotation.Set(Mathf.Lerp(HeadRotation.x, -flashlightRotation.x, verticalSpeed * catchupModifier), HeadRotation.y, HeadRotation.z);
        head.localRotation = Quaternion.Euler(HeadRotation.x, HeadRotation.y, HeadRotation.z);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if(rb != null && !rb.isKinematic)
        {
            rb.velocity = hit.moveDirection;
        }
    }
}
