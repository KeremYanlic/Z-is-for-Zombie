using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementByVelocityEvent))]
[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
public class MovementByVelocity : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private MovementByVelocityEvent movementByVelocityEvent;

    private void Awake()
    {
        //Load components
        playerRB = GetComponent<Rigidbody2D>();
        movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
    }
    private void OnEnable()
    {
        //Subscribe to movement by velocity event
        movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;
    }
    private void OnDisable()
    {
        //Unsubscribe from movement by velocity event
        movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;
    }

    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent movementByVelocityEvent, MovementByVelocityEventArgs movementByVelocityEventArgs)
    {
        MoveRigidbody(movementByVelocityEventArgs);
    }

    // <summary>
    // Move rigidbody
    // </summary>
    private void MoveRigidbody(MovementByVelocityEventArgs movementByVelocityEventArgs)
    {
        Vector2 moveDir = movementByVelocityEventArgs.moveDirection;
        float moveSpeed = movementByVelocityEventArgs.moveSpeed;
        playerRB.velocity = moveDir * moveSpeed;
    }
}
