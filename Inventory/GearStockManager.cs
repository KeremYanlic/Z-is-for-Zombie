using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GearStockManager : SingletonMonobehaviour<GearStockManager>
{
    public bool isGearStockOpen;

    public event Action OnOpenGearStock;
    public event Action OnCloseGearStock;

    private void Start()
    {
        CallCloseGearStock();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && isGearStockOpen)
        {
            //Close
            isGearStockOpen = false;
            OnCloseGearStock?.Invoke();
        }
        else if(Input.GetKeyDown(KeyCode.Tab) && !isGearStockOpen) 
        {
            // Open
            isGearStockOpen = true;

            OnOpenGearStock?.Invoke();
        }
    }

    public void CallOpenGearStock()
    {
        isGearStockOpen = true;
        OnOpenGearStock?.Invoke();
    }
    public void CallCloseGearStock()
    {
        isGearStockOpen = false;
        OnCloseGearStock?.Invoke();
    }
}
