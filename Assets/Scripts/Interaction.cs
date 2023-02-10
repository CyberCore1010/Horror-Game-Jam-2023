using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2;
    [SerializeField] private float forceAmount = 500;
    [SerializeField] private Transform head;

    private GameObject hand, handHover, handGrab;

    private Interactable selectedInteractable;
    private bool holding;

    private Rigidbody selectedRigidbody;
    private Vector3 originalTargetPosition;
    private Vector3 originalRigidbodyPos;
    private float selectionDistance;
    private InputManager inputManager;

    private Door selectedDoor;
    private ItemPickup selectedItem;

    private LayerMask interactionMask;

    // Start is called before the first frame update
    void Start()
    {
        hand = GameObject.Find("Hand");
        handHover = GameObject.Find("HandHover");
        handGrab = GameObject.Find("HandGrab");
        handGrab.SetActive(false);
        interactionMask = LayerMask.GetMask("Interactive");

        inputManager = InputManager.Instance;
        inputManager.playerControls.Default.Interact.performed += context =>
        {
            handHover.SetActive(false);
            handGrab.SetActive(true);

            //TODO: change this to unified method like "Interact()" on base class
            if (selectedInteractable != null)
            {
                holding = true;
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
                else if(selectedInteractable as ItemPickup)
                {
                    Debug.Log("Is Item");
                    selectedRigidbody = GetRigidbodySelected();
                    selectedItem = selectedInteractable as ItemPickup;
                    selectedItem.StartInteraction();
                }
                else if(selectedInteractable as ItemInteractable)
                {
                    Debug.Log("Is Item Interactable");
                    (selectedInteractable as ItemInteractable).StartInteraction();
                    
                }
            }
        };

        inputManager.playerControls.Default.Interact.canceled += context =>
        {
            handHover.SetActive(true);
            handGrab.SetActive(false);
            holding = false;

            selectedRigidbody = null;
            if(selectedDoor != null)
            {
                selectedDoor.DoorRelease();
            }
            selectedDoor = null;

            if(selectedItem != null)
            {
                selectedItem.StopInteraction();
            }
            selectedItem = null;
        };
    }

    private void Update()
    {
        RaycastHit hit;

        if (!holding)
        {
            hand.SetActive(false);
        }
        if (Physics.Raycast(head.position, head.forward, out hit, interactionDistance, interactionMask))
        {
            if (hit.transform.gameObject.TryGetComponent(out selectedInteractable))
            {
                hand.SetActive(true);
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
