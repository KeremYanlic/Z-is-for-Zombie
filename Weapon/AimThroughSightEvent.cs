using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class AimThroughSightEvent : MonoBehaviour
{
    public event Action<AimThroughSightEvent, AimThrouhSightEventArgs> OnAimThroughSight;

    public void CallAimThroughSight(float gunWeight, bool hasGunScope)
    {
        OnAimThroughSight?.Invoke(this, new AimThrouhSightEventArgs() { gunWeight = gunWeight, hasGunScope = hasGunScope });
    }
}

public class AimThrouhSightEventArgs : EventArgs
{
    public float gunWeight;
    public bool hasGunScope;
}
