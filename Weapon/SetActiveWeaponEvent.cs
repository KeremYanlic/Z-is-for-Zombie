using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public class SetActiveWeaponEvent : MonoBehaviour
{
    public event Action<SetActiveWeaponEvent, SetActiveWeaponEventArgs> OnSetActiveWeapon;
    public event Action<SetActiveWeaponEvent> OnEnableWeapon;

    public void CallSetActiveWeaponEvent(Weapon weapon)
    {
        OnSetActiveWeapon?.Invoke(this, new SetActiveWeaponEventArgs() { weapon = weapon });
        OnEnableWeapon?.Invoke(this);
    }
}

public class SetActiveWeaponEventArgs : EventArgs
{
    public Weapon weapon;
}