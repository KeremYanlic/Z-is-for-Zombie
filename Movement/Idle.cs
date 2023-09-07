using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IdleEvent))]
[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
public class Idle : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private IdleEvent idleEvent;

    private void Awake()
    {
        //load components
        playerRB = GetComponent<Rigidbody2D>();
        idleEvent = GetComponent<IdleEvent>();
    }

    private void OnEnable()
    {
        //Subscribe to idle event
        idleEvent.OnIdle += IdleEvent_OnIdle;
    }
    private void OnDisable()
    {
        //Unsubscribe from idle event
        idleEvent.OnIdle -= IdleEvent_OnIdle;
    }

    //<summary>
    //Idle event
    //</summary>
    private void IdleEvent_OnIdle(IdleEvent obj)
    {
        ZeroVelocity();
    }

    //<summary>
    //Set velocity to zero
    //</summary>
    private void ZeroVelocity()
    {
        playerRB.velocity = Vector2.zero;
    }
}
