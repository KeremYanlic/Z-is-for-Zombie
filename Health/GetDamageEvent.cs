using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GetDamageEvent : MonoBehaviour
{
    public event Action<GetDamageEvent, GetDamageEventArgs> OnGetDamage;

    public void CallGetDamageEvent(int damageAmount)
    {
        OnGetDamage?.Invoke(this, new GetDamageEventArgs() { damageAmount = damageAmount });
    }


}
public class GetDamageEventArgs : EventArgs
{
    public int damageAmount;
}

