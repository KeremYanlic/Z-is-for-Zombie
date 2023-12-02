using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : Inventory,ISaveable
{ 
    public static PlayerInventory Instance { get; private set; }

    public event Action<PlayerInventory> OnInventoryUpdated;
    private InventorySlot[] inventorySlots;
    private void Awake()
    {
        Instance = this;

        inventorySlots = new InventorySlot[inventorySize];
    }
    public override bool AddToFirstEmptySlot(InventoryItemSO itemSO, int number)
    {
        int i = FindSlot(itemSO);

        if (i < 0) return false;

        inventorySlots[i].itemSO = itemSO;
        inventorySlots[i].number += number;

        OnInventoryUpdated?.Invoke(this);
        return true;
    }

    public override bool AddItemToSlot(int slot, InventoryItemSO itemSO, int number)
    {
        inventorySlots[slot].itemSO = itemSO;
        inventorySlots[slot].number += number;
        OnInventoryUpdated?.Invoke(this);

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
        OnInventoryUpdated?.Invoke(this);
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

    public override InventoryItemSO GetItemInSlot(int slot)
    {
        return inventorySlots[slot].itemSO;
    }

    public override int GetNumberInSlot(int slot)
    {
        return inventorySlots[slot].number;
    }
    public override int GetInventorySize()
    {
        return inventorySize;
    }
    
    public object CaptureState()
    {
        InventorySlotRecord[] slotStrings = new InventorySlotRecord[inventorySize];
        for(int i = 0; i < inventorySize; i++)
        {
            if(inventorySlots[i].itemSO != null)
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

        for(int i = 0; i< inventorySize; i++)
        {
            inventorySlots[i].itemSO = InventoryItemSO.GetFromID(slotStrings[i].itemID);
            inventorySlots[i].number = slotStrings[i].number;
        }
        OnInventoryUpdated?.Invoke(this);
    }

}
