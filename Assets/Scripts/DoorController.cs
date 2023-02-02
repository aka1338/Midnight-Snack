using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    // If Mama is the one at the DoorTrigger, the door will open automatically. 
    void Start()
    {
        // -90 Left, 90 right, 180 up, 0 down x
        // Just a test to rotate a door. 
        transform.RotateAround(transform.position, new Vector3(0, 0, 1), -90);
    }
}
