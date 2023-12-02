using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItemSO>
{
    [SerializeField] protected InventoryItemIcon inventoryItemIcon;

    protected int index;
    protected InventoryItemSO itemSO;

    public virtual void Setup(int index)
    {
        this.index = index;
    }

    public abstract void AddItems(InventoryItemSO item, int number);
    public abstract InventoryItemSO GetItem();
    public abstract int GetNumber();
    public abstract int MaxAcceptable(InventoryItemSO item);
    public abstract void RemoveItems(int number);
   
}
