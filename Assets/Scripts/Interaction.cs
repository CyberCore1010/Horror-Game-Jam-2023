using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 10;
    [SerializeField] private float forceAmount = 500;
    [SerializeField] private Transform head;

    private Interactable selectedInteractable;

    private Rigidbody selectedRigidbody;
    private Vector3 originalTargetPosition;
    private Vector3 originalRigidbodyPos;
    private float selectionDistance;
    private InputManager inputManager;

    private Door selectedDoor;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
        inputManager.playerControls.Default.Interact.performed += context =>
        {
            if (selectedInteractable != null)
            {
                if(selectedInteractable as Dragable)
                {
                    Debug.Log("Is dragable");
                    selectedRigidbody = GetRigidbodySelected();
                }
                else if(selectedInteractable as Door)
                {
                    Debug.Log("Is door");
                    selectedDoor = selectedInteractable as Door;
                    selectedDoor.DoorHold();
                }
            }
        };

        inputManager.playerControls.Default.Interact.canceled += context =>
        {
            selectedRigidbody = null;
            if(selectedDoor != null)
            {
                selectedDoor.DoorRelease();
            }
            selectedDoor = null;
        };
    }



    private void Update()
    {
        if (Physics.Raycast(head.position, head.forward, out RaycastHit hit, interactionDistance))
        {
            selectedInteractable = hit.transform.gameObject.GetComponent<Interactable>();
            if (selectedInteractable != null)
            {
                Debug.DrawRay(head.position, head.TransformDirection(Vector3.forward) * hit.distance, Color.green);
            }
            else
            {
                Debug.DrawRay(head.position, head.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            }
        }
        else
        {
            Debug.DrawRay(head.position, head.TransformDirection(Vector3.forward) * 100, Color.red);
        }
    }

    private void FixedUpdate()
    {
        if(selectedRigidbody)
        {
            Vector3 positionOffset = (head.position + head.forward * selectionDistance) - originalTargetPosition;
            selectedRigidbody.velocity = (originalRigidbodyPos + positionOffset - selectedRigidbody.transform.position) * forceAmount * Time.deltaTime;
        }
    }

    Rigidbody GetRigidbodySelected()
    {
        if (Physics.Raycast(head.position, head.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.GetComponent<Rigidbody>())
            {
                selectionDistance = Vector3.Distance(head.position, hit.point);
                originalTargetPosition = hit.point;
                originalRigidbodyPos = hit.collider.transform.position;
                return hit.collider.gameObject.GetComponent<Rigidbody>();
            }
        }

        return null;
    }
}
