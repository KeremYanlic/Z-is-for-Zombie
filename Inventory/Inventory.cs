using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// <summary>
// Provides storage for the player inventory. A configurable number of slots are available.
// </summary>
public class Inventory : SingletonMonobehaviour<Inventory> , ISaveable
{
    public event Action<Inventory> OnInventoryUpdated;

    // CONFIG STATE
    #region Tooltip
    [Tooltip("Allowed size")]
    #endregion
    [SerializeField] private int inventorySize = 16;

    public struct InventorySlot
    {
        public InventoryItemSO item;
        public int number;
    }
    private InventorySlot[] slots;

    protected override void Awake()
    {
        base.Awake();

        slots = new InventorySlot[inventorySize];
    }
    // <summary>
    // Attempt to add the items to the first available slot
    // </summary>
    public bool AddToFirstEmptySlot(InventoryItemSO item,int number)
    {
        int i = FindSlot(item);

        if(i < 0)
        {
            return false;
        }
        slots[i].item = item;
        slots[i].number += number;

        OnInventoryUpdated?.Invoke(this);
        return true;

    }

    // <summary>
    // Remove a number of items from the given slot. Will never remove more that there are.
    // </summary>
    public void RemoveFromSlot(int slot,int number)
    {
        slots[slot].number -= number;
        if(slots[slot].number <= 0)
        {
            slots[slot].number = 0;
            slots[slot].item = null;
        }
        OnInventoryUpdated?.Invoke(this);
    }
    // <summary>
    // Will add an item to the given slot if possible. If there is already
    // a stack of this type , it will add to the existing stack. Otherwise,
    // it will be added to the first empty slot.
    // </summary>
    public bool AddItemToSlot(int slot,InventoryItemSO item,int number)
    {
        if(slots[slot].item != null)
        {
            return AddToFirstEmptySlot(item, number);
        }
        var i = FindStack(item);
        if(i >= 0)
        {
            slot = i;
        }
        slots[slot].item = item;
        slots[slot].number += number;
        OnInventoryUpdated?.Invoke(this);

        return true;
    }

    // <summary>
    // Is there an instance of the item in the inventory ?
    // </summary>
    public bool HasItem(InventoryItemSO item)
    {
        for(int i = 0; i< slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i].item, item))
            {
                return true;
            }
        }
        return false;
    }

    // <summary>
    // Return the item type in the given slot.
    // </summary>
    public InventoryItemSO GetItemInSlot(int slot)
    {
        return slots[slot].item;
    }

    // <summary>
    // Get the number of items in the given slot
    // </summary>
    public int GetNumberInSlot(int slot)
    {
        return slots[slot].number;
    }

    // <summary>
    // Could this item fit anywhere in the inventory
    // </summary>
    public bool HasSpaceFor(InventoryItemSO item)
    {
        return FindSlot(item) >= 0;
    }


    // <summary>
    // Find a slot that can accomodate the given item. Returns -1 if no slot is found.
    // </summary>
    private int FindSlot(InventoryItemSO item)
    {
        int i = FindStack(item);
        if(i < 0)
        {
            i = FindEmptySlot();
        }
        return i;
    }


    // <summary>
    // Find an empty slot.Returns -1 if all slots are full.
    // </summary>
    private int FindEmptySlot()
    {
        for(int i = 0; i <slots.Length; i++)
        {
            if(slots[i].item == null)
            {
                return i;
            }
        }
        return -1;
    }


    // <summary>
    // Find an existing stack of this item type. Returns -1 if no stack exists or if the item is not stackable
    // </summary>
    private int FindStack(InventoryItemSO item)
    {
        if (!item.IsStackable())
        {
            return -1;
        }

        for(int i = 0; i < slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i].item, item))
            {
                return i;
            }
        }
        return -1;
    }

    // <summary>
    // How many slots are in the inventory ?
    // </summary>
    public int GetSize()
    {
        return slots.Length;
    }

    [System.Serializable]
    private struct InventorySlotRecord
    {
        public string itemID;
        public int number;
    }


    public object CaptureState()
    {
        var slotStrings = new InventorySlotRecord[inventorySize];
        for(int i = 0; i < inventorySize; i++)
        {
            if(slots[i].item != null)
            {
                slotStrings[i].itemID = slots[i].item.GetItemID();
                slotStrings[i].number = slots[i].number;
            }
        }
        return slotStrings;
    }

    public void RestoreState(object state)
    {
        var slotStrings = (InventorySlotRecord[])state;
        for(int i = 0; i< inventorySize; i++)
        {
            slots[i].item = InventoryItemSO.GetFromId(slotStrings[i].itemID);
            slots[i].number = slotStrings[i].number;
        }
        OnInventoryUpdated?.Invoke(this);

    }

}
