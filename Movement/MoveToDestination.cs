using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
[RequireComponent(typeof(MoveToDestinationEvent))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AIPath))]
[DisallowMultipleComponent]
public class MoveToDestination : MonoBehaviour
{
    private MoveToDestinationEvent moveToDestinationEvent;
    [HideInInspector] public Seeker seeker;
    [HideInInspector] public AIPath aIPath;

  
    private void Awake()
    {
        //Load components
        moveToDestinationEvent = GetComponent<MoveToDestinationEvent>();
        aIPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();       
    }
    
    private void OnEnable()
    {
        //Subscribe to move to destination event
        moveToDestinationEvent.OnMoveToDestination += MoveToDestinationEvent_OnMoveToDestination;

        //Subscribe to stop destination event
        moveToDestinationEvent.OnStopDestination += MoveToDestinationEvent_OnStopDestination;
    }

   
    private void OnDisable()
    {
        //Unsubscribe from move to destination event
        moveToDestinationEvent.OnMoveToDestination -= MoveToDestinationEvent_OnMoveToDestination;

        //Unsubscribe from stop destination event
        moveToDestinationEvent.OnStopDestination -= MoveToDestinationEvent_OnStopDestination;

    }
  
    private void MoveToDestinationEvent_OnMoveToDestination(MoveToDestinationEvent arg1, MoveToDestinationEventArgs moveToDestinationEventArgs)
    {
        aIPath.canMove = true;
        seeker.StartPath(moveToDestinationEventArgs.currentPosition, moveToDestinationEventArgs.targetPosition);
        
        
    }
    private void MoveToDestinationEvent_OnStopDestination(MoveToDestinationEvent moveToDestinationEvent)
    {
        aIPath.canMove = false;
    }
}
