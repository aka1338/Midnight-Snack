using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// TODO - This entire thing needs to be refactored badly. 
public class PlayerController : MonoBehaviour
{
    // Player Controls 
    public Rigidbody2D body;
    public SpriteRenderer spriteRenderer;
    public float walkSpeed;
    public InputAction playerControls;



    // Animation Logic
    public float frameRate;
    float idleTime;

    public List<Sprite> nSprites;
    public List<Sprite> neSprites;
    public List<Sprite> eSprites;
    public List<Sprite> seSprites;
    public List<Sprite> sSprites;

    public Vector2 moveDirection = Vector2.zero;

    // SOUNDS

    // Footsteps 
    int prevFrame = -1;

    private enum CURRENT_TERRAIN { CARPET, TILE, WOOD };
    private enum CURRENT_ROOM { BABY_BEDROOM, MAMA_BEDROOM, BEDROOM_HALLWAY, BATHROOM, LIVINGROOM, KITCHEN, LAUNDRY_ROOM, ENTRANCE_HALLWAY };

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

    [SerializeField]
    private CURRENT_ROOM currentRoom;

    [SerializeField]
    private AK.Wwise.Event footstepsEvent;
    [SerializeField]
    private AK.Wwise.Switch[] terrainSwitch;

    private void OnEnable()
    {
        body = GetComponent<Rigidbody2D>();
        playerControls.Enable();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        DetermineTerrain(collider);
        DetermineRoom(collider); 
    }

    private void DetermineRoom(Collider2D collider)
    {
        // Can change this to a switch later but this was just faster to copy/paste out 
        if (collider.gameObject.CompareTag("MamaBedroom"))
        {
            currentRoom = CURRENT_ROOM.MAMA_BEDROOM;    
        }
        if (collider.gameObject.CompareTag("BabyBedroom"))
        {
            currentRoom = CURRENT_ROOM.BABY_BEDROOM;
        }
        if (collider.gameObject.CompareTag("BedroomHallway"))
        {
            currentRoom = CURRENT_ROOM.BEDROOM_HALLWAY;
        }
        if (collider.gameObject.CompareTag("Bathroom"))
        {
            currentRoom = CURRENT_ROOM.BATHROOM;
        }
        if (collider.gameObject.CompareTag("EntranceHallway"))
        {
            currentRoom = CURRENT_ROOM.ENTRANCE_HALLWAY;

        }
        if (collider.gameObject.CompareTag("LaundryRoom"))
        {
            currentRoom = CURRENT_ROOM.LAUNDRY_ROOM;
        }
        if (collider.gameObject.CompareTag("Livingroom"))
        {
            currentRoom = CURRENT_ROOM.LIVINGROOM;
        }
        if (collider.gameObject.CompareTag("Kitchen"))
        {
            currentRoom = CURRENT_ROOM.KITCHEN;
        }
        Debug.Log(currentRoom); 
    }

    private void DetermineTerrain(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Wood"))
        {
            currentTerrain = CURRENT_TERRAIN.WOOD;
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Carpet"))
        {
            currentTerrain = CURRENT_TERRAIN.CARPET;
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Tile"))
        {
            currentTerrain = CURRENT_TERRAIN.TILE;
        }
    }

    public void SelectAndPlayFootstep()
    {
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.CARPET:
                PlayFootstep(0);
                break;

            case CURRENT_TERRAIN.TILE:
                PlayFootstep(1);
                break;

            case CURRENT_TERRAIN.WOOD:
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
        moveDirection = playerControls.ReadValue<Vector2>().normalized;
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
