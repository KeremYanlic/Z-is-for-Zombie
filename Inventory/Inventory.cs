using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    #region Tooltip
    [Tooltip("Allowed inventory size")]
    #endregion
    [SerializeField] protected int inventorySize = 48;
    public struct InventorySlot
    {
        public InventoryItemSO itemSO;
        public int number;
    }
    public struct InventorySlotRecord
    {
        public string itemID;
        public int number;
    }
  
    public abstract bool AddToFirstEmptySlot(InventoryItemSO itemSO, int number);
    public abstract bool AddItemToSlot(int slot, InventoryItemSO itemSO, int number);
    public abstract void RemoveFromSlot(int slot, int number);

    public abstract bool HasItem(InventoryItemSO itemSO);

    public abstract InventoryItemSO GetItemInSlot(int slot);
    public abstract int GetNumberInSlot(int slot);

    public abstract int FindStack(InventoryItemSO itemSO);
    public abstract int FindEmptySlot();
    public abstract int GetInventorySize();
    public virtual bool HasSpaceFor(InventoryItemSO itemSO)
    {
        return FindSlot(itemSO) >= 0;
    }
    
    protected int FindSlot(InventoryItemSO itemSO)
    {
        int i = FindStack(itemSO);
        if (i < 0)
        {
            i = FindEmptySlot();
        }
        return i;
    }
}
