using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton
// Contains the logic for dispatching events related to gameplay. 
public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<int> onDoorwayTriggerEnter;

    public void DoorwayTriggerEnter(int id)
    {
        if (onDoorwayTriggerEnter != null)
        {
            onDoorwayTriggerEnter(id);
        }
    }

    public event Action<int> onDoorwayTriggerExit;
 
    public void DoorwayTriggerExit(int id)
    {
        if (onDoorwayTriggerExit != null)
        {
            onDoorwayTriggerExit(id);
        }
    }
}
