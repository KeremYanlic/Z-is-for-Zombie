using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DisableWeaponEvent : MonoBehaviour
{
    public event Action<DisableWeaponEvent> OnDisableWeapon;

    public void CallDisableWeaponEvent()
    {
        OnDisableWeapon?.Invoke(this);
    }
}
