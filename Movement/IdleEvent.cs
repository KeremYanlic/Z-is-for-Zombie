using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class IdleEvent : MonoBehaviour
{
    public event Action<IdleEvent> OnIdle;


    //<summary>
    //Idle event
    //</summary>
    public void CallIdleEvent()
    {
        OnIdle?.Invoke(this);
    }
}
