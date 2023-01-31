using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public InputAction playerControls;
    public Vector2 moveDirection = Vector2.zero;
    public float moveSpeed = 5f;

    private enum CURRENT_TERRAIN { CARPET, TILE, WOOD };

    float timer = 0.0f;

    [SerializeField]
    float footstepSpeed = 0.3f;

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

    [SerializeField]
    private AK.Wwise.Event footstepsEvent;

    [SerializeField]
    private AK.Wwise.Switch[] terrainSwitch;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Baby is walking on " + collider.gameObject.layer);

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

        // Default case 
        else
        {
            currentTerrain = CURRENT_TERRAIN.WOOD;
        }

        Debug.Log(currentTerrain); 
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

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls.Enable();
        
    }

    private void Update()
    {
        moveDirection = playerControls.ReadValue<Vector2>();

        if (rb.velocity.magnitude > 1f && Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            if (timer > footstepSpeed)
            {
                SelectAndPlayFootstep();
                timer = 0.0f;
            }

            timer += Time.deltaTime;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed * Time.deltaTime,
                                  moveDirection.y * moveSpeed * Time.deltaTime);
    }
}
