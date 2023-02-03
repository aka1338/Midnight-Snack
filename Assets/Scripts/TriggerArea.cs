using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    // TODO - This class should also contain the information of what side the entity is coming from to open the door appropriately. 
    // Alternatively, we can just set the Door to disable it's box collider while the door is open. 
    public bool playerInTriggerArea = false;
    public bool isDoorClosed = true;

    // This ID is tied to the enum CURRENT_ROOM.  
    public int id; 
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController.current.SetDoorTriggerArea(id); 
        playerInTriggerArea = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInTriggerArea = false;

    }
}
