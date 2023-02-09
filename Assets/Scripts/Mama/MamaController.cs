using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Singleton
public class MamaController : MonoBehaviour
{
    public static MamaController current;

    private Vector2 moveDirection = Vector2.zero;
    NavMeshAgent agent;


    [Header("Mama State")]
    public ROOM currentRoom;
    public TERRAIN currentTerrain;
    public MAMA_STATE mamaState = MAMA_STATE.IDLE;
    public int currentDoorTrigger;
    
    [Header("Navigation Targets")]
    public GameObject player;
    public GameObject mamaBed;
    public List<GameObject> lightSwitches;
    public Stack<GameObject> flippedSwitches = new Stack<GameObject>();
    public GameObject currentNavTarget;
    public float positionTolerance = 0.5f;

    // Animation Logic
    float idleTime;

    [Header("Mama Sprites")]
    public float frameRate;
    public SpriteRenderer spriteRenderer;
    public List<Sprite> nSprites;
    public List<Sprite> neSprites;
    public List<Sprite> eSprites;
    public List<Sprite> seSprites;
    public List<Sprite> sSprites;

    int prevFrame = -1;

    [Header("Wwise")]
    [SerializeField]
    private AK.Wwise.Event footstepsEvent;
    [SerializeField]
    private AK.Wwise.Switch[] terrainSwitch;

  
    private void Start()
    {
        GameEvents.current.onNoiseEventTriggerEnter += FlipLightOnInAlertedRoom;
    }

    private void OnDisable()
    {
        GameEvents.current.onNoiseEventTriggerEnter -= FlipLightOnInAlertedRoom;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        current = this;
    }

    internal void SetDoorTriggerArea(int id)
    {
        currentDoorTrigger = id;
    }

    // Animation and SFX 
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

    private List<Sprite> GetSpriteDirection()
    {
        List<Sprite> selectedSprites = null;

        if (moveDirection.y > 0) // north
        {
            if (Mathf.Abs(moveDirection.x) > 0) // east or west
            {
                // Uncomment this if we implement 8-Way directionals
                //selectedSprites = neSprites;
                selectedSprites = nSprites;
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
                selectedSprites = sSprites;

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

    private void DetermineRoom(Collider2D collider)
    {
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

    private void HandleMamaWalkingAnimation()
    {
        moveDirection = agent.velocity.normalized;

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

    // AI Logic 
    IEnumerator PauseAtLightAndTrackPlayer()
    {
        yield return new WaitForSecondsRealtime(2);

        // Flip on the light of the room Mama is currently in 
        GameEvents.current.MamaTurnLightOn(currentRoom);

        mamaState = MAMA_STATE.SEEKING_PLAYER;
        GameManager.current.SetMamaState(mamaState);
        currentNavTarget = PlayerController.current.gameObject;
        agent.SetDestination(currentNavTarget.transform.position);
    }

    IEnumerator WaitAndReturnToBed()
    {
        yield return new WaitForSecondsRealtime(5);
        mamaState = MAMA_STATE.RETURNING_TO_BED;
        GameManager.current.SetMamaState(mamaState);
        currentNavTarget = mamaBed;
        agent.SetDestination(mamaBed.transform.position);
    }

    public void ReturnToBed()
    {
        currentNavTarget = mamaBed;
        agent.SetDestination(mamaBed.transform.position);
    }

    public void FlipLightOnInAlertedRoom(ROOM room)
    {
        mamaState = MAMA_STATE.ALERTED;
        GameManager.current.SetMamaState(mamaState);

        // Take the room and navigate to the lightswitch GameObject of the corresponding room. 
        if (room == ROOM.BABY_BEDROOM)
        {
            FlipLightswitchOn(lightSwitches[0]);
        }
        if (room == ROOM.MAMA_BEDROOM)
        {
            FlipLightswitchOn(lightSwitches[1]);
        }
        if (room == ROOM.BATHROOM)
        {
            FlipLightswitchOn(lightSwitches[2]);
        }
        if (room == ROOM.LAUNDRY_ROOM)
        {
            FlipLightswitchOn(lightSwitches[3]);
        }
        if (room == ROOM.ENTRANCE_HALLWAY)
        {
            FlipLightswitchOn(lightSwitches[4]);
        }
        if (room == ROOM.OFFICE)
        {
            FlipLightswitchOn(lightSwitches[5]);
        }
        if (room == ROOM.LIVINGROOM)
        {
            FlipLightswitchOn(lightSwitches[6]);
        }
        if (room == ROOM.KITCHEN)
        {
            FlipLightswitchOn(lightSwitches[6]);
        }
    }

    private void FlipLightswitchOn(GameObject lightSwitch)
    {
        currentNavTarget = lightSwitch;
        agent.SetDestination(lightSwitch.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        DetermineTerrain(collider);
        DetermineRoom(collider);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Turn off the light in this room. 
    }

    void Update()
    {
        HandleMamaWalkingAnimation();

        // When Mom gets a noise event, she will come into the room the player is on and flip the light on.
        // She first navigates to the light and flips the light on or off. 
        if (agent.hasPath)
        {
            if (Vector2.Distance(transform.position, currentNavTarget.transform.position) < positionTolerance) // Mama Arrived at Target Location
            {
                Debug.Log("Mama arrived to position.");

                if (currentNavTarget.name.Contains("Lightswitch")) // Arrived at a lightswitch. Sound event goes here.
                {
                    if (mamaState == MAMA_STATE.ALERTED)
                    {
                        StartCoroutine(PauseAtLightAndTrackPlayer());
                    }
                }

                if (currentNavTarget.name.Contains("Bed")) // Arrived to bed.
                {
                    mamaState = MAMA_STATE.IDLE;
                    GameManager.current.SetMamaState(mamaState);
                    GameEvents.current.MamaTurnLightOff(currentRoom);
                }

            }
        }
        else
        {
            if (currentNavTarget != null)
            {
                if (currentNavTarget.name.Contains("Player"))
                {
                    StartCoroutine(WaitAndReturnToBed());
                }
            }
        }

        // Animation 

    }

}
