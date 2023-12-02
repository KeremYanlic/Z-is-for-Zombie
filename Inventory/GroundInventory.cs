using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GroundInventory : Inventory, ISaveable
{
    public static GroundInventory Instance { get; private set; }

    public event Action<GroundInventory> OnGarbageInventoryUpdated;
    public event Action<GroundInventory, OnGroundInventoryClosedEventArgs> OnCreatePickup;

    private InventorySlot[] inventorySlots;

    private void Awake()
    {
        Instance = this;

        inventorySlots = new InventorySlot[inventorySize];
    }

    private void Start()
    {
        ShowHideUI.Instance.OnOpenInventory += OnOpenInventory;
        ShowHideUI.Instance.OnCloseInventory += OnCloseInventory;
        ShowHideUI.Instance.OnOpenPickup += OnOpenPickup;
        ShowHideUI.Instance.OnClosePickup += OnClosePickup;
    }
    private void OnDestroy()
    {
        ShowHideUI.Instance.OnOpenInventory -= OnOpenInventory;
        ShowHideUI.Instance.OnCloseInventory -= OnCloseInventory;
        ShowHideUI.Instance.OnOpenPickup -= OnOpenPickup;
        ShowHideUI.Instance.OnClosePickup -= OnClosePickup;
    }

    private void OnOpenInventory(ShowHideUI showHideUI)
    {
        OnGarbageInventoryUpdated?.Invoke(this);
    }
    private void OnCloseInventory(ShowHideUI showHideUI)
    {
        if (IsThereItem() == false) return;

        CreatePickupEvent(transform.position, CaptureState());
        ResetInventory();
        CaptureState();
        OnGarbageInventoryUpdated?.Invoke(this);
    }
    private void OnOpenPickup(ShowHideUI showHideUI, OnOpenPickupEventArgs onOpenPickupEventArgs)
    {
        object pickupInventoryState = onOpenPickupEventArgs.pickupSpawner.CaptureState();
        RestoreState(pickupInventoryState);

        OnGarbageInventoryUpdated?.Invoke(this);
    }

    private void OnClosePickup(ShowHideUI showHideUI, OnClosePickupEventArgs onClosePickupEventArgs)
    {
        PickupSpawner pickupSpawner = onClosePickupEventArgs.pickupSpawner;

        if (IsThereItem() == false)
        {
            pickupSpawner.DestroyPickup();
            return;
        }
        
        object pickupLastState = CaptureState();
        pickupSpawner.LoadInventory(pickupLastState);

        ResetInventory();
        OnGarbageInventoryUpdated?.Invoke(this);
    }

    private void CreatePickupEvent(Vector3 pickupPos, object pickupInventoryState)
    {
        OnCreatePickup?.Invoke(this, new OnGroundInventoryClosedEventArgs()
        {
            position = pickupPos,
            grbInventoryState = pickupInventoryState
        });
    }
    public override bool AddToFirstEmptySlot(InventoryItemSO itemSO, int number)
    {
        int i = FindEmptySlot();

        if (i < 0) return false;

        inventorySlots[i].itemSO = itemSO;
        inventorySlots[i].number += number;

        OnGarbageInventoryUpdated?.Invoke(this);
        return true;
    }
    public override bool AddItemToSlot(int slot, InventoryItemSO itemSO, int number)
    {
        inventorySlots[slot].itemSO = itemSO;
        inventorySlots[slot].number += number;
        OnGarbageInventoryUpdated?.Invoke(this);
        return true;
    }

  
    public override void RemoveFromSlot(int slot, int number)
    {
        inventorySlots[slot].number -= number;
        if (inventorySlots[slot].number <= 0)
        {
            inventorySlots[slot].itemSO = null;
            inventorySlots[slot].number = 0;
        }
        OnGarbageInventoryUpdated?.Invoke(this);
    }
    public override bool HasItem(InventoryItemSO itemSO)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (object.ReferenceEquals(inventorySlots[i].itemSO, itemSO))
            {
                return true;
            }
        }
        return false;
    }
    public override int FindEmptySlot()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].itemSO == null)
            {
                return i;
            }
        }
        return -1;
    }
    public override int FindStack(InventoryItemSO itemSO)
    {
        if (!itemSO.IsStackable())
        {
            return -1;
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (object.ReferenceEquals(inventorySlots[i].itemSO, itemSO))
            {
                return i;
            }
        }
        return -1;
    }

    private bool IsThereItem()
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            if(inventorySlots[i].itemSO != null)
            {
                return true;
            }
        }
        return false;
    }
    public override int GetInventorySize()
    {
        return inventorySize;
    }

    public override InventoryItemSO GetItemInSlot(int slot)
    {
        return inventorySlots[slot].itemSO;
    }

    public override int GetNumberInSlot(int slot)
    {
        return inventorySlots[slot].number;
    }
    public object CaptureState()
    {
        InventorySlotRecord[] slotStrings = new InventorySlotRecord[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventorySlots[i].itemSO != null)
            {
                slotStrings[i].itemID = inventorySlots[i].itemSO.GetItemID();
                slotStrings[i].number = inventorySlots[i].number;
            }
        }
        return slotStrings;
    }

    public void RestoreState(object state)
    {
        InventorySlotRecord[] slotStrings = (InventorySlotRecord[])state;

        for (int i = 0; i < inventorySize; i++)
        {
            inventorySlots[i].itemSO = InventoryItemSO.GetFromID(slotStrings[i].itemID);
            inventorySlots[i].number = slotStrings[i].number;
        }
        OnGarbageInventoryUpdated?.Invoke(this);
    }
    private void ResetInventory()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventorySlots[i].itemSO == null) continue;

            inventorySlots[i].itemSO = null;
            inventorySlots[i].number = 0;

        }
    }
}

public class OnGroundInventoryClosedEventArgs : EventArgs 
{
    public Vector3 position;
    public object grbInventoryState;
}
