using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(RunEvent))]
[RequireComponent(typeof(GetDamageEvent))]
[DisallowMultipleComponent]
public class PlayerStatus : MonoBehaviour, IDamageable
{

    private RunEvent runEvent;
    private GetDamageEvent getDamageEvent;

    private int startingHealth;
    private int currentHealth;

    private float startingStamina;
    private float currentStamina;
    private bool isRunning = false;

    private float timerToIncreaseStamina;
    private float timerToIncreaseStaminaMax = 2f;
    [SerializeField] private float staminaDecreaseMultiplier = 10f;

    private void Awake()
    {
        //Load components
        runEvent = GetComponent<RunEvent>();
        getDamageEvent = GetComponent<GetDamageEvent>();
    }

    private void OnEnable()
    {
        //Subscribe to get damage event
        getDamageEvent.OnGetDamage += GetDamageEvent_OnGetDamage;

        //Subscribe to run event
        runEvent.OnRun += RunEvent_OnRun;

        //Subscribe to on not run event
        runEvent.OnNotRun += RunEvent_OnNotRun;

    }


    private void OnDisable()
    {
        //Unsubscribe from get damage event
        getDamageEvent.OnGetDamage -= GetDamageEvent_OnGetDamage;

        //Unsubscribe from run event
        runEvent.OnRun -= RunEvent_OnRun;

        //Unsubscribe from on not run event
        runEvent.OnNotRun -= RunEvent_OnNotRun;
    }


    private void Update()
    {
        //If player is not running then regenerate the stamina after two seconds of delay.
        if (isRunning) return;

        timerToIncreaseStamina -= Time.deltaTime;

        if (timerToIncreaseStamina <= 0f)
        {
            IncreaseStamina();
        }
    }
    //<summary>
    //Player health get damage handler
    //</summary>
    private void GetDamageEvent_OnGetDamage(GetDamageEvent getDamageEvent, GetDamageEventArgs getDamageEventArgs)
    {
        GetDamage((int)getDamageEventArgs.damageAmount);
    }
    //<summary>
    //Player run event handler. The aim of this event is decreasing the current stamina
    //</summary>
    private void RunEvent_OnRun(RunEvent obj)
    {
        isRunning = true;

        timerToIncreaseStamina = timerToIncreaseStaminaMax;

        //Decrease stamina
        DecreaseStamina();

    }
    //<summary>
    //Player not run event handler. The aim of this event is decreasing the current stamina
    //</summary>
    private void RunEvent_OnNotRun(RunEvent obj)
    {
        isRunning = false;
    }
    //<summary>
    //Get damage method
    //</summary>
    public void GetDamage(int damageAmount)
    {
        if (currentHealth <= 0f)
        {
            currentHealth = 0;
            return;
        }


        currentHealth -= damageAmount;
    }
    //<summary>
    //Decrease the current stamina
    //</summary>
    private void DecreaseStamina()
    {
        if (currentStamina <= 0f)
        {
            currentStamina = 0;
            return;
        }
        currentStamina -= staminaDecreaseMultiplier * Time.deltaTime;
    }
    //<summary>
    //Decrease the current stamina by the value that we give
    //</summary>
    public void DecreaseStamina(float staminaToSpend)
    {
        //Decrease the stamina
        currentStamina -= staminaToSpend;

        //Reset stamina regenerate timer
        timerToIncreaseStamina = timerToIncreaseStaminaMax;
    }
    public bool IsThereStaminaToAction(float staminaToSpend)
    {
        //Lets say staminaToSpend value is 30 then our current stamina should be at least 31 in order to call this function;
        return currentStamina - staminaToSpend > 0f;
    }

    //<summary>
    //Increase the current stamina
    //</summary>
    private void IncreaseStamina()
    {
        if (currentStamina > startingStamina)
        {
            currentStamina = startingStamina;
            return;
        }
        currentStamina += staminaDecreaseMultiplier * Time.deltaTime;
    }
    public void InitializePlayerHealth(PlayerDetailsSO playerDetailsSO)
    {
        startingHealth = playerDetailsSO.GetStartingHealth();
        currentHealth = startingHealth;
    }
    public void InitializePlayerStamina(PlayerDetailsSO playerDetailSO)
    {
        startingStamina = playerDetailSO.GetStartingStamina();
        currentStamina = startingStamina;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public int GetStartingHealth()
    {
        return startingHealth;
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }
}
