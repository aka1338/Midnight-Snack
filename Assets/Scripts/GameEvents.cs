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
    public event Action<int> onDoorwayTriggerExit;

    // Mama should subscribe to this event. 
    public event Action<int> onNoiseEventTriggerEnter;

    // When the snack is obtained 
    public event Action onSnackObtained;

    public void NoiseEventTriggerEnter(int id)
    {
        if (onNoiseEventTriggerEnter != null)
        {
            onNoiseEventTriggerEnter(id);
        }
    }

    public void SnackObtained()
    {
        if (onSnackObtained != null)
        {
            onSnackObtained();
        }
    }

    public void DoorwayTriggerEnter(int id)
    {
        if (onDoorwayTriggerEnter != null)
        {
            onDoorwayTriggerEnter(id);
        }
    }
 
    public void DoorwayTriggerExit(int id)
    {
        if (onDoorwayTriggerExit != null)
        {
            onDoorwayTriggerExit(id);
        }
    }
}
