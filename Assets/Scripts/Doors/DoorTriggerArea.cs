using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerArea : MonoBehaviour
{
    // Alternatively, we can just set the Door to disable it's box collider while the door is open. 
    public bool playerInTriggerArea = false;
    public bool isDoorClosed = true;
    public bool mamaInTriggerArea = false; 

    // This ID is tied to the enum CURRENT_ROOM.  
    public int id; 
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Player"))
        {
            PlayerController.current.SetDoorTriggerArea(id);
            playerInTriggerArea = true;
        }
        if (collision.name.Contains("Mama"))
        {
            Debug.Log("Mama collided with door trigger area."); 
            MamaController.current.SetDoorTriggerArea(id);
            GameEvents.current.MamaDoorwayTriggerEnter(id); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Contains("Player"))
        {
            playerInTriggerArea = false;
        }
        if (collision.name.Contains("Mama"))
        {
            Debug.Log("Mama exited door trigger area.");
            MamaController.current.SetDoorTriggerArea(id);
            GameEvents.current.MamaDoorwayTriggerExit(id);
        }
    }
}
