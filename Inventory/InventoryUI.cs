using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(GearStockManager))]
[DisallowMultipleComponent]
public class InventoryUI : SingletonMonobehaviour<InventoryUI>
{
    private GearStockManager gearStockManager;


    public event Action OnOpenInventory;
    public event Action OnCloseInventory;
    public event Action<InventoryUI, OnOpenPickupEventArgs> OnOpenPickup;
    public event Action<InventoryUI,OnClosePickupEventArgs> OnClosePickup;

    [SerializeField] private GameObject playerInventoryBackground;
    [SerializeField] private GameObject groundInventoryBackground;

    protected override void Awake()
    {
        // load components
        gearStockManager = GetComponent<GearStockManager>();
    }

    private void OnEnable()
    {
        // Subscribe to open gear stock event
        gearStockManager.OnOpenGearStock += GearStockManager_OnOpenGearStock;

        // Subscribe to close gear stock event
        gearStockManager.OnCloseGearStock += GearStockManager_OnCloseGearStock;
    }

 
    private void OnDisable()
    {
        // Unsubscribe from open gear stock event
        gearStockManager.OnOpenGearStock -= GearStockManager_OnOpenGearStock;

        // Unsubscribe from close gear stock event
        gearStockManager.OnCloseGearStock -= GearStockManager_OnCloseGearStock;
    }


    // <summary>
    // Open inventory event
    // </summary>
    private void GearStockManager_OnOpenGearStock()
    {
        //Open inventory
        OpenInventory();
    }
    // <summary>
    // Close inventory event
    // </summary>
    private void GearStockManager_OnCloseGearStock()
    {
        //Close inventory
        CloseInventory();
    }



    public void CallOpenPickupEvent(PickupSpawner pickupSpawner)
    {
        OnOpenPickup?.Invoke(this, new OnOpenPickupEventArgs() { pickupSpawner = pickupSpawner });
    }
    public void CallClosePickupEvent(PickupSpawner pickupSpawner)
    {
        OnClosePickup?.Invoke(this, new OnClosePickupEventArgs() { pickupSpawner = pickupSpawner });
    }



    public void OpenInventory()
    {
        playerInventoryBackground.SetActive(true);
        groundInventoryBackground.SetActive(true);
    }
    public void CloseInventory()
    {
        playerInventoryBackground.SetActive(false);
        groundInventoryBackground.SetActive(false);
    }
}

public class OnOpenPickupEventArgs : EventArgs
{
    public PickupSpawner pickupSpawner;
}

public class OnClosePickupEventArgs : EventArgs
{
    public PickupSpawner pickupSpawner;
}
