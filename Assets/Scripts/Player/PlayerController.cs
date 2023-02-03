using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// TODO - This entire thing needs to be refactored badly. 
public class PlayerController : MonoBehaviour
{

    // TODO: Make the ROOM enum public. 

    [SerializeField]
    private TERRAIN currentTerrain;

    [SerializeField]
    private ROOM currentRoom;
    private int currentDoorTrigger; 

    internal void SetDoorTriggerArea(int id)
    {
        currentDoorTrigger = id;
    }

    // Player Controls 
    public Rigidbody2D body;
    public SpriteRenderer spriteRenderer;
    public float walkSpeed;
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction interact;
    public Vector2 moveDirection = Vector2.zero;

    // Animation Logic
    public float frameRate;
    float idleTime;

    public List<Sprite> nSprites;
    public List<Sprite> neSprites;
    public List<Sprite> eSprites;
    public List<Sprite> seSprites;
    public List<Sprite> sSprites;

    // SOUNDS

    // Footsteps 
    int prevFrame = -1;
    [SerializeField]
    private AK.Wwise.Event footstepsEvent;
    [SerializeField]
    private AK.Wwise.Switch[] terrainSwitch;

    public static PlayerController current;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        current = this;

    }

    private void OnEnable() 
    {
        playerControls.Enable();

        // Player Move 
        move = playerControls.Player.Move;
        move.Enable(); 

        // Player Interact
        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;

        // Rigidbody interactions 
        body = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        move.Disable();
        interact.Disable(); 
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        DetermineTerrain(collider);
        DetermineRoom(collider); 

        // If the collider is equal to the snack, move to the next game phase. 
        if(collider.gameObject.name == "Snack")
        {
            Debug.Log("Snack obtained!");
            GameEvents.current.SnackObtained(); 
        }
    }

    private void DetermineRoom(Collider2D collider)
    {
        // Can change this to a switch later but this was just faster to copy/paste out 
        if (collider.gameObject.CompareTag("MamaBedroom"))
        {
            currentRoom = ROOM.MAMA_BEDROOM;    
        }
        if (collider.gameObject.CompareTag("BabyBedroom"))
        {
            currentRoom = ROOM.BABY_BEDROOM;
        }
        if (collider.gameObject.CompareTag("BedroomHallway"))
        {
            currentRoom = ROOM.BEDROOM_HALLWAY;
        }
        if (collider.gameObject.CompareTag("Bathroom"))
        {
            currentRoom = ROOM.BATHROOM;
        }
        if (collider.gameObject.CompareTag("EntranceHallway"))
        {
            currentRoom = ROOM.ENTRANCE_HALLWAY;

        }
        if (collider.gameObject.CompareTag("LaundryRoom"))
        {
            currentRoom = ROOM.LAUNDRY_ROOM;
        }
        if (collider.gameObject.CompareTag("Livingroom"))
        {
            currentRoom = ROOM.LIVINGROOM;
        }
        if (collider.gameObject.CompareTag("Kitchen"))
        {
            currentRoom = ROOM.KITCHEN;
        }
        Debug.Log(currentRoom); 
    }

    private void DetermineTerrain(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Wood"))
        {
            currentTerrain = TERRAIN.WOOD;
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Carpet"))
        {
            currentTerrain = TERRAIN.CARPET;
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Tile"))
        {
            currentTerrain = TERRAIN.TILE;
        }
    }

    public void SelectAndPlayFootstep()
    {
        switch (currentTerrain)
        {
            case TERRAIN.CARPET:
                PlayFootstep(0);
                break;

            case TERRAIN.TILE:
                PlayFootstep(1);
                break;

            case TERRAIN.WOOD:
                PlayFootstep(2);
                break;

            default:
                PlayFootstep(0);
                break;
        }
    }

    private void PlayFootstep(int terrain)
    {
        terrainSwitch[terrain].SetValue(gameObject);
        AkSoundEngine.PostEvent(footstepsEvent.Id, this.gameObject);
    }

    private void Update()
    {
        moveDirection = move.ReadValue<Vector2>().normalized;
        body.velocity = moveDirection * walkSpeed;

        HandleSpriteFlip();

        List<Sprite> directionSprites = GetSpriteDirection();

        if (directionSprites != null) // holding a direction 
        {
            
            float playTime = Time.time - idleTime;
            if (playTime != 0)
            {
                int totalFrames = (int)(playTime * frameRate);
                int frame = totalFrames % directionSprites.Count;
                spriteRenderer.sprite = directionSprites[frame];

                if (frame != prevFrame)
                {
                    prevFrame = frame;
                    SelectAndPlayFootstep();
                }

            }

        }
        else // holding nothing 
        {
            idleTime = Time.time;
        }
    }
    
    // TODO: This might have to reference the current TriggerArea the player is inside of. 
    private void Interact(InputAction.CallbackContext context)
    {
        GameEvents.current.DoorwayTriggerEnter((int)currentDoorTrigger); 
    }
    private List<Sprite> GetSpriteDirection()
    {
        List<Sprite> selectedSprites = null;

        if (moveDirection.y > 0) // north
        {
            if (Mathf.Abs(moveDirection.x) > 0) // east or west
            {
                // Uncomment this if we implement 8-Way directionals
                //selectedSprites = neSprites;
            }
            else
            { // neutral x 
                selectedSprites = nSprites;
            }
        }
        else if (moveDirection.y < 0) // south 
        {
            if (Mathf.Abs(moveDirection.x) > 0) // east or west
            {
                // Uncomment this if we implement 8 - Way directionals
                //selectedSprites = seSprites;
            }
            else
            { // neutral x 
                selectedSprites = sSprites;
            }
        }
        else // neutral
        {
            if (Math.Abs(moveDirection.x) > 0)
            {
                selectedSprites = eSprites;
            }
        }

        return selectedSprites;
    }

    private void HandleSpriteFlip()
    {
        if (!spriteRenderer.flipX && moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (spriteRenderer.flipX && moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}