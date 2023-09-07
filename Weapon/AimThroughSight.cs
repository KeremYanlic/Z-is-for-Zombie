using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AimThrouhSightEventArgs))]
[DisallowMultipleComponent]
public class AimThroughSight : MonoBehaviour
{
    private AimThroughSightEvent aimThroughSightEvent;

    private void Awake()
    {
        //Load components
        aimThroughSightEvent = GetComponent<AimThroughSightEvent>();
    }

    private void OnEnable()
    {
        aimThroughSightEvent.OnAimThroughSight += AimThroughSightEvent_OnAimThroughSight;
    }
    private void OnDisable()
    {
        aimThroughSightEvent.OnAimThroughSight -= AimThroughSightEvent_OnAimThroughSight;
    }
    private void AimThroughSightEvent_OnAimThroughSight(AimThroughSightEvent aimThroughSightEvent, AimThrouhSightEventArgs aimThrouhSightEventArgs)
    {

    }


}
