using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RunEvent : MonoBehaviour
{
    public event Action<RunEvent> OnRun;
    public event Action<RunEvent> OnNotRun;
    public void CallRunEvent()
    {
        OnRun?.Invoke(this);
    }
    public void CallOnNotRun()
    {
        OnNotRun?.Invoke(this);
    }
}
