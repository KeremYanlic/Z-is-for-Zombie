using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MoveToDestinationEvent : MonoBehaviour
{

    public event Action<MoveToDestinationEvent, MoveToDestinationEventArgs> OnMoveToDestination;
    public event Action<MoveToDestinationEvent> OnStopDestination;

    public void CallMoveToDestinationEvent(Vector3 currentPosition,Vector3 targetPosition,Vector2 moveDirection)
    {
        OnMoveToDestination?.Invoke(this, new MoveToDestinationEventArgs() { currentPosition = currentPosition, targetPosition = targetPosition,moveDirection = moveDirection });
    }
    public void CallStopDestinationEvent()
    {
        OnStopDestination?.Invoke(this);
    }
}
public class MoveToDestinationEventArgs : EventArgs
{
    public Vector3 currentPosition;
    public Vector3 targetPosition;
    public Vector2 moveDirection;
}
