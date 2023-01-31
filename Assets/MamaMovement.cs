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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false; 
    }

    // Update is called once per frame
    void Update()
    {
        SetTargetPosition();
        SetAgentPosition(); 
    }

    private void SetAgentPosition()
    {
        target = player.transform.position; 
    }

    private void SetTargetPosition()
    {
        agent.SetDestination(new Vector3(target.x, target.y)); 
    }

}
