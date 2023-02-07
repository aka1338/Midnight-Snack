using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseEvent : MonoBehaviour
{
    public int noiseID; // ID Tied to the noise this NoiseEvent should emit. 
    public ROOM roomID; 

    public float cooldownTime = 2f;
    private bool onCooldown;
    private float cooldownTimer;
    public bool isOneShotEvent = false;
    public bool oneShotEventTriggered = false;

    private void Update()
    {
        if (onCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                onCooldown = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (!onCooldown && !oneShotEventTriggered) // if the event is not on cooldown, play the event. 
            {
                if (isOneShotEvent)
                {
                    oneShotEventTriggered = true;
                }
                GameEvents.current.NoiseEventTriggerEnter(roomID); 
                onCooldown = true;
                cooldownTimer = cooldownTime;
            }
        }
    }
}
