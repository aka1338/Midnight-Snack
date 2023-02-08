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

    public event Action<int> onMamaDoorwayTriggerEnter;
    public event Action<int> onMamaDoorwayTriggerExit;

    // Mama should subscribe to this event. 
    public event Action<ROOM> onNoiseEventTriggerEnter;

    // When the snack is obtained 
    public event Action onSnackObtained;

    public event Action onPlayerEnterHidingSpot;
    public event Action onPlayerExitHidingSpot;


    public event Action<ROOM> onMamaTurnLightOn;
    public event Action<ROOM> onMamaTurnLightOff;

    public void MamaTurnLightOn(ROOM room)
    {
        if (onMamaTurnLightOn != null)
        {
            onMamaTurnLightOn(room);
        }
    }
    public void MamaTurnLightOff(ROOM room)
    {
        if (onMamaTurnLightOff != null)
        {
            onMamaTurnLightOff(room);
        }
    }


    public void PlayerEnterHidingSpot()
    {
        if (onPlayerEnterHidingSpot != null)
        {
            onPlayerEnterHidingSpot();
        }
    }

    public void PlayerExitHidingSpot()
    {
        if (onPlayerExitHidingSpot != null)
        {
            onPlayerExitHidingSpot();
        }
    }

    public void NoiseEventTriggerEnter(ROOM roomID)
    {
        if (onNoiseEventTriggerEnter != null)
        {
            onNoiseEventTriggerEnter(roomID);
        }
    }

    public void MamaDoorwayTriggerEnter(int id)
    {
        if (onMamaDoorwayTriggerEnter != null)
        {
            onMamaDoorwayTriggerEnter(id);
        }
    }

    public void MamaDoorwayTriggerExit(int id)
    {
        if (onMamaDoorwayTriggerExit != null)
        {
            onMamaDoorwayTriggerExit(id);
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
