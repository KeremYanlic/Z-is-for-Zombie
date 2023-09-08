using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SetActiveCarEvent : MonoBehaviour
{
    public event Action<SetActiveCarEvent, SetActiveCarEventArgs> OnSetActiveCar;

    public event Action<SetActiveCarEvent> OnDeactiveCar;
    public void CallSetActiveCar(GameObject activeCar)
    {
        OnSetActiveCar?.Invoke(this, new SetActiveCarEventArgs() { activeCar = activeCar });
    }

    public void CallDeactiveCar()
    {
        OnDeactiveCar?.Invoke(this);
    }


}
public class SetActiveCarEventArgs : EventArgs
{
    public GameObject activeCar;
}
