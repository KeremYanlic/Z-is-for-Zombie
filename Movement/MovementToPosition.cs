using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementToPositionEvent))]
[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
public class MovementToPosition : MonoBehaviour
{
    private MovementToPositionEvent movementToPositionEvent;
    private Rigidbody2D playerRB;

    private void Awake()
    {
        //Load components
        playerRB = GetComponent<Rigidbody2D>();
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();
    }

    private void OnEnable()
    {
        //Subscribe to onMovementToPosition event
        movementToPositionEvent.OnMovementToPosition += MovementToPositionEvent_OnMovementToPosition;
    }
    private void OnDisable()
    {
        //Unsubscribe from onMovementToPosition event
        movementToPositionEvent.OnMovementToPosition -= MovementToPositionEvent_OnMovementToPosition;
    }

    // <summary>
    // Movement to position event
    // </summary>
    private void MovementToPositionEvent_OnMovementToPosition(MovementToPositionEvent movementToPositionEvent, MovementToPositionEventArgs movementToPositionEventArgs)
    {
        MoveRigidbody(movementToPositionEventArgs.movePosition, movementToPositionEventArgs.currentPosition, movementToPositionEventArgs.moveSpeed);
    }

    // <summary>
    // Move the rigidbody2D
    // </summary>
    private void MoveRigidbody(Vector3 movePosition, Vector3 currentPosition, float moveSpeed)
    {
        Vector3 unitVector = (movePosition - currentPosition).normalized;

        playerRB.MovePosition(playerRB.position + ((Vector2)unitVector * moveSpeed * Time.fixedDeltaTime));
    }
}
