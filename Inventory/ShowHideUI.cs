using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShowHideUI : SingletonMonobehaviour<ShowHideUI>
{
    public event Action<ShowHideUI> OnOpenInventory;
    public event Action<ShowHideUI> OnCloseInventory;
    public event Action<ShowHideUI, OnOpenPickupEventArgs> OnOpenPickup;
    public event Action<ShowHideUI,OnClosePickupEventArgs> OnClosePickup;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject garbagePanel;

    public bool isInventoryOpen = false;

    private void Start()
    {
        CloseInventory();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isInventoryOpen)
        {
            OpenInventory();
            isInventoryOpen = true;

            OnOpenInventory?.Invoke(this);
        }
        else if(Input.GetKeyDown(KeyCode.Tab) && isInventoryOpen)
        {
            CloseInventory();
            isInventoryOpen = false;
            OnCloseInventory?.Invoke(this);
        }
       
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
        inventoryPanel.SetActive(true);
        garbagePanel.SetActive(true);
    }
    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        garbagePanel.SetActive(false);
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
