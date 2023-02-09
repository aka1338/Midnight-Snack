using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public DoorTriggerArea triggerArea;
    public BoxCollider2D doorCollider;

    [Header("Door Open Rotation Position")]
    public int openRotationDirectionX;
    public int openRotationDirectionY;
    public int openRotationDirectionZ;

    [Header("Door Close Rotation Position")]
    public int closeRotationDirectionX;
    public int closeRotationDirectionY;
    public int closeRotationDirectionZ;

    [Header("Wwise")]
    [SerializeField]
    private AK.Wwise.Event doorOpenEvent;
    [SerializeField]
    private AK.Wwise.Event doorCloseEvent;

    [Header("Door ID")]
    // If the DoorController and the TriggerArea id's match, that's what determines which door is interactable. 
    public int id; 
   
    // If Mama is the one at the DoorTrigger, the door will open automatically. 
    void Start()
    {
        GameEvents.current.onDoorwayTriggerEnter += OnPlayerDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit += OnPlayerDoorwayClose;
        GameEvents.current.onMamaDoorwayTriggerEnter += MamaOpenDoor;
        GameEvents.current.onMamaDoorwayTriggerExit += MamaCloseDoor;

    }

    private void OnDestroy()
    {
        GameEvents.current.onDoorwayTriggerEnter -= OnPlayerDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit -= OnPlayerDoorwayClose;
        GameEvents.current.onMamaDoorwayTriggerEnter -= MamaOpenDoor;
        GameEvents.current.onMamaDoorwayTriggerExit -= MamaCloseDoor;
    }

    private void OnPlayerDoorwayOpen(int id)
    {
        if (triggerArea != null && id == this.id)
        {
            if (triggerArea.playerInTriggerArea && triggerArea.isDoorClosed)
            {
                Vector3 rotateVector = new Vector3(openRotationDirectionX, openRotationDirectionY, openRotationDirectionZ);
                transform.DORotate(rotateVector, 1f);
                triggerArea.isDoorClosed = false;

                // Sound FX
                AkSoundEngine.PostEvent(doorOpenEvent.Id, this.gameObject);

                GameEvents.current.NoiseEventTriggerEnter((ROOM)id);
                // Not sure if I like this or not, still debating. 
                DisableDoorCollider();  
            }
            else
            {
                OnPlayerDoorwayClose(id);
            }
        }
    }
    private void OnPlayerDoorwayClose(int id)
    {
        if (triggerArea != null && id == this.id)
        {
            if (triggerArea.playerInTriggerArea && !triggerArea.isDoorClosed)
            {
                // We should get the door open orientation from the door. 
                Vector3 rotateVector = new Vector3(closeRotationDirectionX, closeRotationDirectionY, closeRotationDirectionZ);
                transform.DORotate(rotateVector, 1f).onComplete = EnableDoorCollider;
                GameEvents.current.NoiseEventTriggerEnter((ROOM)id);
                triggerArea.isDoorClosed = true;
            }
            else
            {
                if (triggerArea.playerInTriggerArea)
                    OnPlayerDoorwayOpen(id);
            }
        }
    }

    // If the gameObject calling OnDoorwayOpen is Mama, the door should open automatically. 
    public void MamaOpenDoor(int id)
    {
        if (triggerArea != null && id == this.id)
        {
            if (triggerArea.isDoorClosed) 
            {
                Vector3 rotateVector = new Vector3(openRotationDirectionX, openRotationDirectionY, openRotationDirectionZ);
                transform.DORotate(rotateVector, 1f);
                triggerArea.isDoorClosed = false;
                AkSoundEngine.PostEvent(doorOpenEvent.Id, this.gameObject);
                DisableDoorCollider();
            }
        }
    }

    public void MamaCloseDoor(int id)
    {
        if (triggerArea != null && id == this.id)
        {
            if (!triggerArea.isDoorClosed)
            {
                // We should get the door open orientation from the door. 
                Vector3 rotateVector = new Vector3(closeRotationDirectionX, closeRotationDirectionY, closeRotationDirectionZ);
                transform.DORotate(rotateVector, 1f).onComplete = EnableDoorCollider;
                triggerArea.isDoorClosed = true;
            }
        }
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
