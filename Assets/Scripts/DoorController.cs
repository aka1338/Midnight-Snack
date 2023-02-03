using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public TriggerArea triggerArea;
    public BoxCollider2D doorCollider;
    public int openRotationDirection;
    public int closeRotationDirection;


    // If the DoorController and the TriggerArea id's match, that's what determines which door is interactable. 
    public int id; 
   
    // If Mama is the one at the DoorTrigger, the door will open automatically. 
    void Start()
    {
        GameEvents.current.onDoorwayTriggerEnter += OnDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit += OnDoorwayClose;
    }

    private void OnDestroy()
    {
        GameEvents.current.onDoorwayTriggerEnter -= OnDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit -= OnDoorwayClose;
    }

    private void OnDoorwayOpen(int id)
    {
        if (triggerArea != null && id == this.id)
        {
            if (triggerArea.playerInTriggerArea && triggerArea.isDoorClosed)
            {
                Vector3 rotateVector = new Vector3(0, 0, openRotationDirection);
                transform.DORotate(rotateVector, 1f);
                triggerArea.isDoorClosed = false;

                // Not sure if I like this or not, still debating. 
                DisableDoorCollider();  
            }
            else
            {
                OnDoorwayClose(id);
            }
        }
    }

    private void OnDoorwayClose(int id)
    {
        if(triggerArea != null && id == this.id)
        {
            if (triggerArea.playerInTriggerArea && !triggerArea.isDoorClosed)
            {
                // We should get the door open orientation from the door. 
                Vector3 rotateVector = new Vector3(0, 0, closeRotationDirection);
                transform.DORotate(rotateVector, 1f).onComplete = EnableDoorCollider;
                triggerArea.isDoorClosed = true;
            }
            else
            {
                if (triggerArea.playerInTriggerArea)
                    OnDoorwayOpen(id);
            }
        }
        // If the gameObject calling OnDoorwayOpen is Mama, the door should open automatically. 
    }

    private void DisableDoorCollider()
    {
        doorCollider.enabled = false;

    }

    private void EnableDoorCollider()
    {
        doorCollider.enabled = true; 
    }

}
