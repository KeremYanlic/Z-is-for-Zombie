using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementByVelocityEvent : MonoBehaviour
{
    public event Action<MovementByVelocityEvent, MovementByVelocityEventArgs> OnMovementByVelocity;

    //<summary>
    //Movement by velocity event
    //</summary>
    public void CallMovementByVelocityEvent(Vector2 moveDirection, float moveSpeed)
    {
        OnMovementByVelocity?.Invoke(this, new MovementByVelocityEventArgs() { moveDirection = moveDirection, moveSpeed = moveSpeed });
    }
}

//<summary>
//Movement by velocity event args
//<summary>
public class MovementByVelocityEventArgs : EventArgs
{
    public Vector2 moveDirection;
    public float moveSpeed;
}
