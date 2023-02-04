using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private float forceAmount = 500;
    [SerializeField] private Transform head;

    private Interactable selectedInteractable;

    private Rigidbody selectedRigidbody;
    private Vector3 originalTargetPosition;
    private Vector3 originalRigidbodyPos;
    private float selectionDistance;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
        inputManager.playerControls.Default.Interact.started += context =>
        {
            if (selectedInteractable != null)
            {
                if(selectedInteractable as Dragable)
                {
                    selectedRigidbody = GetRigidbodySelected();
                }
            }
        };

        inputManager.playerControls.Default.Interact.performed += context =>
        {
            Debug.Log("Dropped");
            selectedRigidbody = null;
        };
    }



    private void Update()
    {
        if (Physics.Raycast(head.position, head.forward, out RaycastHit hit, Mathf.Infinity))
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
            Debug.Log("Dragging");
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
                Debug.Log("FoundRigid");
                selectionDistance = Vector3.Distance(head.position, hit.point);
                originalTargetPosition = hit.point;
                originalRigidbodyPos = hit.collider.transform.position;
                return hit.collider.gameObject.GetComponent<Rigidbody>();
            }
        }

        return null;
    }
}
