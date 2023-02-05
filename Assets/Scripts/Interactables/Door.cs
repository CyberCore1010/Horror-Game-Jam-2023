using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private bool dragging;
    private Transform frame;
    private Transform player;
    private InputManager inputManager;
    private HingeJoint hinge;
    private float turnangle;

    public void Start()
    {
        frame = transform.parent;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inputManager = InputManager.Instance;
        hinge = GetComponent<HingeJoint>();
    }

    public void DoorHold()
    {
        dragging = true;
    }

    public void DoorRelease()
    {
        dragging = false;
    }

    public void FixedUpdate()
    {
        JointMotor motor = hinge.motor;

        if (dragging)
        {
            if (inputManager.GetMouseDelta().x > 0)
            {
                turnangle = -90;
            }
            else if (inputManager.GetMouseDelta().x < 0)
            {
                turnangle = 90;
            }
            else
            {
                turnangle = 0;
            }

            Vector3 relativePoint = frame.InverseTransformPoint(player.position);
            if(relativePoint.z > 0)
            {
                motor.force = 5;
                motor.targetVelocity = turnangle;
            }
            else if (relativePoint.z < 0)
            {
                motor.force = 5;
                motor.targetVelocity = -turnangle;
            }
            
        }
        else
        {
            motor.force = 1;
        }
        hinge.motor = motor;
    }
}
