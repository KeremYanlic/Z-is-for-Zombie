using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WeaponLight : MonoBehaviour
{
    private AimThroughSightEvent aimThroughSightEvent;

    private Light2D weaponLightRight;
    private Light2D weaponLightLeft;
    private bool isOpen = true;
    private void Awake()
    {
        aimThroughSightEvent = gameObject.GetComponentInParent<Player>().GetComponent<AimThroughSightEvent>();

        weaponLightRight = transform.GetChild(0).GetComponent<Light2D>();
        weaponLightLeft = transform.GetChild(1).GetComponent<Light2D>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        //Subscribe to aim through sight event
        aimThroughSightEvent.OnAimThroughSight += AimThroughSightEvent_OnAimThroughSight;
    }
    private void OnDisable()
    {
        //Unubscribe from aim through sight event
        aimThroughSightEvent.OnAimThroughSight -= AimThroughSightEvent_OnAimThroughSight;
    }

    private void AimThroughSightEvent_OnAimThroughSight(AimThroughSightEvent arg1, AimThrouhSightEventArgs aimThrouhSightEventArgs)
    {
        if (aimThrouhSightEventArgs.hasGunScope)
        {
            CloseWeaponLights();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ManageWeaponLight();
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
}
