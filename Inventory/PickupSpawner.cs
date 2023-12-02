using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : Inventory, ISaveable
{
    public InventorySlot[] inventorySlots;

    private void Awake()
    {
        inventorySlots = new InventorySlot[inventorySize];
    }
  
    public void LoadInventory(object capturedState)
    {
        RestoreState(capturedState);
    }
    public void DestroyPickup()
    {
        Destroy(gameObject);
    }
   
 
    public override bool AddItemToSlot(int slot, InventoryItemSO itemSO, int number)
    {
        inventorySlots[slot].itemSO = itemSO;
        inventorySlots[slot].number = number;

        return true;
    }
    public override void RemoveFromSlot(int slot, int number)
    {
        inventorySlots[slot].itemSO = null;
        inventorySlots[slot].number = number;
    }

    public override bool AddToFirstEmptySlot(InventoryItemSO itemSO, int number)
    {
        throw new System.NotImplementedException();
    }

    public override int FindEmptySlot()
    {
        throw new System.NotImplementedException();
    }

    public override int FindStack(InventoryItemSO itemSO)
    {
        throw new System.NotImplementedException();
    }

    public override int GetInventorySize()
    {
        throw new System.NotImplementedException();
    }

    public override InventoryItemSO GetItemInSlot(int slot)
    {
        throw new System.NotImplementedException();
    }

    public override int GetNumberInSlot(int slot)
    {
        throw new System.NotImplementedException();
    }

    public override bool HasItem(InventoryItemSO itemSO)
    {
        throw new System.NotImplementedException();
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
    }
}
