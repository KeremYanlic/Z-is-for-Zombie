using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class ActiveCar : MonoBehaviour
{
    private Player player;

    [HideInInspector] public GameObject activeCar;

    private void Awake()
    {

        //Load components
        player = GetComponent<Player>();

    }
    private void OnEnable()
    {
        //Subscribe to set active car event
        player.setActiveCarEvent.OnSetActiveCar += SetActiveCarEvent_OnSetActiveCar;

        //Subscribe to deactive car event
        player.setActiveCarEvent.OnDeactiveCar += SetActiveCarEvent_OnDeactiveCar;
    }

    private void OnDisable()
    {
        //Unubscribe to set active car event
        player.setActiveCarEvent.OnSetActiveCar -= SetActiveCarEvent_OnSetActiveCar;

        //Unsubscribe from deactive car event
        player.setActiveCarEvent.OnDeactiveCar -= SetActiveCarEvent_OnDeactiveCar;
    }

    private void SetActiveCarEvent_OnSetActiveCar(SetActiveCarEvent arg1, SetActiveCarEventArgs setActiveCarEventArgs)
    {
        //Set the active car.
        activeCar = setActiveCarEventArgs.activeCar;

        Car car = setActiveCarEventArgs.activeCar.GetComponent<Car>();
        car.isCarActive = true;

        //Disable player.
        player.playerController.DisablePlayer();
    }

    private void SetActiveCarEvent_OnDeactiveCar(SetActiveCarEvent obj)
    {
        //Get car variable from active car.
        Car car = activeCar.GetComponent<Car>();

        //Player leaves the car so make him enable.
        player.playerController.EnablePlayer();


        player.transform.position = car.GetPlayerSpawnPosition();
        car.isCarActive = false;

        //Set active car to null
        activeCar = null;


    }

}
