using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    bool isFollowingPlayer = false;
    bool canFollowPlayer = true;
    Transform startingPosition;

    public Transform player;
    public float speed = 1f;
    public float bobbingAmplitude = 1f;
    public float bobbingFrequency = 1f;
    private float startTime;

    private void Start()
    {
        startTime = Time.time;

        startingPosition = transform; 
        GameEvents.current.onMamaTurnLightOn += ResetMonsterPosition;
        GameEvents.current.onMamaTurnLightOff += MamaturnLightOff;
    }

    private void Update()
    {
        if (canFollowPlayer && isFollowingPlayer)
        {
            // Move the object towards the player
            transform.position = Vector3.Lerp(transform.position, player.position, speed * Time.deltaTime);

            // Calculate the bobbing effect
            float bobbingY = Mathf.Sin((Time.time - startTime) * bobbingFrequency) * bobbingAmplitude;
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + bobbingY, transform.position.z);
            transform.position = newPosition;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("PlayerController"))
        {
            if (canFollowPlayer)
            {
                isFollowingPlayer = true;
            }
        }
    }

    public void ResetMonsterPosition(ROOM room)
    {
        // Place the monster back in it's starting position
        transform.position = startingPosition.position;
        canFollowPlayer = false; 
        // Set the monster to can't follow player 
    }

    public void MamaturnLightOff(ROOM room)
    {
        // Set the monster to can follow player!
        canFollowPlayer = true; 
    }
}
