using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewRadiusController : MonoBehaviour
{
    private float timeInTrigger = 0f;

    [Header("View Properties")]
    public bool isDark = true;
    bool isHiding = false;

    private CircleCollider2D darkViewRadius;
    private BoxCollider2D litViewRadius;

    [Header("Colliders")]
    public BoxCollider2D playerCollider;

    [Header("Sprites")]
    public SpriteMask playerDarkViewMask;
    public SpriteRenderer darkness;

    [Header("Gameplay Elements (Detection)")]
    public float darkDetectionTime = 4f;
    public float litDetectionTime = 2f;

    // Have an event that listens for the light to turn on, and if the player is hiding or not. 

    private void Start()
    {
        darkViewRadius = this.GetComponent<CircleCollider2D>();
        litViewRadius = this.GetComponent<BoxCollider2D>();
        litViewRadius.enabled = false;
        GameEvents.current.onPlayerEnterHidingSpot += PlayerEnteredHidingSpot;
        GameEvents.current.onPlayerExitHidingSpot += PlayerExitedHidingSpot;
        GameEvents.current.onMamaTurnLightOn += EnableLitRadius;
        GameEvents.current.onMamaTurnLightOff += EnableDarkRadius;
    }

    private void EnableDarkRadius(ROOM room)
    {
        Debug.Log("LIGHTS OFF");
        // If the player is in the same room this event triggered from, enable. 

        isDark = true;
        if (isHiding)
        {
            darkViewRadius.enabled = false;
            playerDarkViewMask.enabled = false;
        }
        litViewRadius.enabled = false;

        darkness.enabled = true;
    }

    private void EnableLitRadius(ROOM obj)
    {
        Debug.Log("LIGHTS ON");
        // If the player is in the same room this event triggered from, enable. 
        if (obj == PlayerController.current.currentRoom)
        {
            isDark = false;
            darkViewRadius.enabled = false;
            darkness.enabled = false;
        }
    }

    private void PlayerExitedHidingSpot()
    {
        isHiding = false;
        playerDarkViewMask.enabled = true;
        darkViewRadius.enabled = isDark; // If it's dark, give back the dark view radius. 
        litViewRadius.enabled = !isDark; // If it's not dark, give the lit radius. 
    }

    private void PlayerEnteredHidingSpot()
    {
        isHiding = true;
        playerDarkViewMask.enabled = false;
        darkViewRadius.enabled = false;
        litViewRadius.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        timeInTrigger += Time.fixedDeltaTime;
        if (timeInTrigger >= litDetectionTime && collision.gameObject.name == "Mama" && !isDark)
        {
            Debug.Log("Caught player whole room was lit!");
        }
        if (timeInTrigger >= darkDetectionTime && collision.gameObject.name == "Mama")
        {
            // Game over! 
            Debug.Log("Caught player while room was dark!");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        timeInTrigger = 0f;
    }

}
