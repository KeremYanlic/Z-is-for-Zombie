using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[DisallowMultipleComponent]
public class PlayerDetecter : SingletonMonobehaviour<PlayerDetecter>
{
    private const float X_DETECT_SIZE = 35f;
    private const float Y_DETECT_SIZE = 25f;

    private Player player;
   
    private Collider2D[] detectedCars;
    private Collider2D[] detectedZombies;
    private Collider2D[] detectedPickupSpawners;
    
    public bool interactingPickupSpawner = false;

    [SerializeField] private LayerMask collidableLayers;
    protected override void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        //Check for near cars.
        detectedCars = Physics2D.OverlapCircleAll(transform.position, Settings.carDetectRadius, Settings.carLayer);

        //Check for near zombies
        detectedZombies = Physics2D.OverlapBoxAll(transform.position, new Vector2(X_DETECT_SIZE, Y_DETECT_SIZE), 360, Settings.zombieLayer);

        //Check for near pickup spawners
        detectedPickupSpawners = Physics2D.OverlapCircleAll(transform.position, Settings.pickupSpawnerDetectRadius, Settings.pickupSpawnerLayer);
     
        //Check environment for a car and press F to ride that.
        SetActiveCar();
    }
    private void LateUpdate()
    {
        if (IsThereZombie())
        {
            //There are some zombies!!
            ArrangeZombiesVisibilities();
        }
        if (Input.GetKeyDown(KeyCode.F) && IsTherePickup() && !interactingPickupSpawner && !ShowHideUI.Instance.isInventoryOpen)
        {
            player.playerController.DisableMovement();

            ShowHideUI.Instance.OpenInventory();
            ShowHideUI.Instance.isInventoryOpen = true;
            interactingPickupSpawner = true;

            GameObject pickupGameobject = UtilsClass.GetClosestGameobject(detectedPickupSpawners);
            PickupSpawner pickupSpawner = pickupGameobject.GetComponent<PickupSpawner>();

            ShowHideUI.Instance.CallOpenPickupEvent(pickupSpawner);
        }


        else if (Input.GetKeyDown(KeyCode.F) && interactingPickupSpawner && ShowHideUI.Instance.isInventoryOpen)
        {
            player.playerController.EnableMovement();

            ShowHideUI.Instance.CloseInventory();
            ShowHideUI.Instance.isInventoryOpen = false;
            interactingPickupSpawner = false;

            GameObject pickupGameobject = UtilsClass.GetClosestGameobject(detectedPickupSpawners);
            PickupSpawner pickupSpawner = pickupGameobject.GetComponent<PickupSpawner>();

            ShowHideUI.Instance.CallClosePickupEvent(pickupSpawner);
        }

    }
    //<summary>
    //Set the active car
    //</summary>
    private void SetActiveCar()
    {
        //Button for interacting with cars
        bool interactWithCar = Input.GetKeyDown(KeyCode.F);

        if (interactWithCar && player.activeCar.activeCar == null)
        {   
            //If there is any car then drive it.
            if (IsThereCar())
            {
                //Get closest car near to player
                GameObject closestCar = UtilsClass.GetClosestGameobject(detectedCars);
                //Set nearest car as current car.
                player.setActiveCarEvent.CallSetActiveCar(closestCar);
                
            }
        
        }
        //If player already rides a car and if player press F then player leaves the car.
        else if(interactWithCar && player.activeCar.activeCar != null)
        {
            //Call deactive car event
            player.setActiveCarEvent.CallDeactiveCar();
        }
    }

    //<summary>
    //Arrange zombies visibilities based on the character's look angle
    //<summary>
    private void ArrangeZombiesVisibilities()
    {
        foreach (Collider2D collider in detectedZombies)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, collider.gameObject.transform.position - transform.position, Mathf.Infinity, collidableLayers);

            if (hit.collider.gameObject != collider.gameObject && !hit.collider.gameObject.CompareTag(Settings.zombieTag))
            {
                ZombieInvisibleProcess(collider.gameObject);
            }
            else
            {

                ZombieVisibleProcess(collider.gameObject);
            }
        }
    }

    //<summary>
    //Turn zombies invisible objects when character doesnt see them
    //<summary>
    private void ZombieInvisibleProcess(GameObject gameobject)
    {
        ZombieController zombieController = gameobject.gameObject.GetComponent<ZombieController>();
        zombieController.ZombieInvisibleProcess();

    }
    //<summary>
    //Turn zombies visible objects when character does see them
    //</summary>
    private void ZombieVisibleProcess(GameObject gameObject)
    {
        ZombieController zombieController = gameObject.gameObject.GetComponent<ZombieController>();
        zombieController.ZombieVisibleProcess();
    }



    //<summary>
    //Check if there any car close to us.
    //</summary>
    public bool IsThereCar()
    {
        return detectedCars.Length > 0;
    }

    //<summary>
    //Check if there any zombie close to us.
    //</summary>
    private bool IsThereZombie()
    {
        return detectedZombies.Length > 0;
    }
    public bool IsTherePickup()
    {
        return detectedPickupSpawners.Length > 0;
    } 
}
