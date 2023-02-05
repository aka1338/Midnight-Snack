using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MamaMovement : MonoBehaviour
{
    public GameObject player; 
    private Vector3 target;
    NavMeshAgent agent;

    public GameObject mamaBedPosition; 
    public List<GameObject> lightSwitches;
    private Vector3 startingPosition;

    private void Start()
    {
        GameEvents.current.onNoiseEventTriggerEnter += MamaSwitchLightOn;
        startingPosition = transform.position;
    }

    private void OnDisable()
    {
        
    }
    // MamaWalkTo()
    // MamaSwitchLightOn(Room room) 
    // MamaAlerted()
    // MamaReturnToBed() 

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false; 
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    public void MamaSwitchLightOn(ROOM room)
    {
        Debug.Log("Alert from " + room); 
        // Take the room and navigate to the lightswitch GameObject of the corresponding room. 
        if (room == ROOM.BEDROOM_HALLWAY)
        {
            Debug.Log("Mama navigating to Bedroom Hallway lightswitch."); 
            NavigateToLightSwitch(lightSwitches[0].transform.localPosition); 
        }
        if (room == ROOM.BABY_BEDROOM)
        {
            NavigateToLightSwitch(lightSwitches[1].transform.localPosition);
        }
        if (room == ROOM.MAMA_BEDROOM)
        {
            NavigateToLightSwitch(lightSwitches[2].transform.localPosition);

        }
        if (room == ROOM.BATHROOM)
        {
            NavigateToLightSwitch(lightSwitches[3].transform.localPosition);

        }
        if (room == ROOM.LAUNDRY_ROOM)
        {
            NavigateToLightSwitch(lightSwitches[4].transform.localPosition);

        }
        if (room == ROOM.KITCHEN)
        {
            NavigateToLightSwitch(lightSwitches[5].transform.localPosition);

        }
        if (room == ROOM.LIVINGROOM)
        {
            NavigateToLightSwitch(lightSwitches[6].transform.localPosition);

        }

    }

    private void NavigateToLightSwitch(Vector3 position)
    {
        agent.SetDestination(position);
        if (Vector3.Distance(transform.position, position) < 0.5f)
        {
            Debug.Log("Destination reached."); 
            StartCoroutine(WaitAndReturn(position));
        }
    }

    private IEnumerator WaitAndReturn(Vector3 lightSwitchPosition)
    {
        agent.SetDestination(player.transform.position);
        yield return new WaitForSeconds(5f);
        agent.SetDestination(lightSwitchPosition);
        if (Vector3.Distance(transform.position, lightSwitchPosition) < 0.5f)
        {
            agent.SetDestination(mamaBedPosition.transform.position);
        }
    }
}
