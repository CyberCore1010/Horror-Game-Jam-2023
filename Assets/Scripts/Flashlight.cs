using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Vector3 current;
    private Vector3 lookPoint;
    private float speed;

    public void PointAt(Vector3 point, float speed)
    {
        lookPoint = point;
        if(current == null)
        {
            current = point;
        }
        this.speed = speed;
    }

    public void FixedUpdate()
    {
        current = Vector3.Lerp(current, lookPoint, (speed * 0.5f) * Time.deltaTime);
        transform.LookAt(current);
    }
}
