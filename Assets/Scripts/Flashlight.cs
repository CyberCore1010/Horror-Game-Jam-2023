using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public void PointAt(Vector3 point)
    {
        transform.LookAt(point);
    }
}
