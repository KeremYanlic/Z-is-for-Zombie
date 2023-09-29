using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WeaponLight : MonoBehaviour
{
    private AimThroughSightEvent aimThroughSightEvent;
    private DisableWeaponEvent disableWeaponEvent;
    private SetActiveWeaponEvent setActiveWeaponEvent;

    private Light2D weaponLightRight;
    private Light2D weaponLightLeft;
    private bool isWeaponActive = true;
    private bool isOpen = true;
    private void Awake()
    {
        //Load components
        aimThroughSightEvent = gameObject.GetComponentInParent<Player>().GetComponent<AimThroughSightEvent>();
        disableWeaponEvent = gameObject.GetComponentInParent<Player>().GetComponent<DisableWeaponEvent>();
        setActiveWeaponEvent = gameObject.GetComponentInParent<Player>().GetComponent<SetActiveWeaponEvent>();

        weaponLightRight = transform.GetChild(0).GetComponent<Light2D>();
        weaponLightLeft = transform.GetChild(1).GetComponent<Light2D>();
    }
   
    private void OnEnable()
    {
        //Subscribe to aim through sight event
        aimThroughSightEvent.OnAimThroughSight += AimThroughSightEvent_OnAimThroughSight;

        //Subscribe to disable weapon event
        disableWeaponEvent.OnDisableWeapon += DisableWeaponEvent_OnDisableWeapon;

        //Subscribe to enable weapon event
        setActiveWeaponEvent.OnEnableWeapon += SetActiveWeaponEvent_OnEnableWeapon;
    }

   
    private void OnDisable()
    {
        //Unubscribe from aim through sight event
        aimThroughSightEvent.OnAimThroughSight -= AimThroughSightEvent_OnAimThroughSight;

        //Unubscribe from disable weapon event
        disableWeaponEvent.OnDisableWeapon -= DisableWeaponEvent_OnDisableWeapon;

        //Unsubscribe from enable weapon event
        setActiveWeaponEvent.OnEnableWeapon -= SetActiveWeaponEvent_OnEnableWeapon;
    }


    // Update is called once per frame
    void Update()
    {
        if (!isWeaponActive) return;
        ManageWeaponLight();
    }


    private void AimThroughSightEvent_OnAimThroughSight(AimThroughSightEvent arg1, AimThrouhSightEventArgs aimThrouhSightEventArgs)
    {
        if (aimThrouhSightEventArgs.hasGunScope)
        {
            CloseWeaponLights();
        }
    }

    private void DisableWeaponEvent_OnDisableWeapon(DisableWeaponEvent obj)
    {
        isWeaponActive = false;
        if (isOpen)
        {
            CloseWeaponLights();
        }
    }

    private void SetActiveWeaponEvent_OnEnableWeapon(SetActiveWeaponEvent obj)
    {
        isWeaponActive = true;
        if (!isOpen)
        {
            OpenWeaponLights();
        }
    }


  

    //<summary>
    //Manage the weapon's ligth.
    //</summary>
    private void ManageWeaponLight()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OpenCloseLight();
        }
    }
   
    private void OpenCloseLight()
    {
        isOpen = !isOpen;

        weaponLightRight.enabled = isOpen;
        weaponLightLeft.enabled = isOpen;
    }
    private void CloseWeaponLights()
    {
        if (isOpen)
        {
            isOpen = false;
            weaponLightRight.enabled = isOpen;
            weaponLightLeft.enabled = isOpen;
        }
    }
    private void OpenWeaponLights()
    {
        if (!isOpen)
        {
            isOpen = true;
            weaponLightRight.enabled = isOpen;
            weaponLightLeft.enabled = isOpen;
        }
    }
    
}
