using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(GetDamageEvent))]
public class ZombieStatus : MonoBehaviour, IDamageable
{
    [HideInInspector] public int health;
    [HideInInspector] public GetDamageEvent getDamageEvent;


    private void Awake()
    {
        //Load components
        getDamageEvent = GetComponent<GetDamageEvent>();
    }
    private void OnEnable()
    {
        //Subscribe to get damage event
        getDamageEvent.OnGetDamage += GetDamageEvent_OnGetDamage;
    }
    private void OnDisable()
    {
        //Unsubscribe from get damage event
        getDamageEvent.OnGetDamage -= GetDamageEvent_OnGetDamage;

    }

    private void GetDamageEvent_OnGetDamage(GetDamageEvent getDamageEvent, GetDamageEventArgs getDamageEventArgs)
    {
        GetDamage(getDamageEventArgs.damageAmount);
    }

    public void GetDamage(int damageAmount)
    {
        //Decrease health
        health -= damageAmount;
        Debug.Log(health);

        if (health <= 0f)
        {
            //Disable this gameobject if zombie's health is lower than zero
            Disable();
        }


    }

    //<summary>
    //Disable gameobject
    //</summary>
    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
