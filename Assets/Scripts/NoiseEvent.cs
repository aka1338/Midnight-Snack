using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseEvent : MonoBehaviour
{
    [Header("Noise Properties")]
    public ROOM roomID;
    public float cooldownTime = 2f;
   
    // Set isOneShotEvent to true if this noise should trigger one time only. 
    public bool isOneShotEvent = false;
    private bool oneShotEventTriggered = false;

    // Cooldown for triggering this noise again
    private bool onCooldown;
    private float cooldownTimer;

    [Header("Wwise")]
    [SerializeField]
    private AK.Wwise.Event noiseEvent;

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
                AkSoundEngine.PostEvent(noiseEvent.Id, this.gameObject);
                onCooldown = true;
                cooldownTimer = cooldownTime;
            }
        }
        else if (collision.gameObject.name == "Mama")
        {
            AkSoundEngine.PostEvent(noiseEvent.Id, this.gameObject);
            onCooldown = true;
            cooldownTimer = cooldownTime;
        }
    }
}
