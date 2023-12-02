using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlotUI : SlotUI
{
    public override void Setup(int index)
    {
        base.Setup(index);
        inventoryItemIcon.SetItem(GroundInventory.Instance.GetItemInSlot(index), GroundInventory.Instance.GetNumberInSlot(index));       
    }

    public override void AddItems(InventoryItemSO item, int number)
    {
        GroundInventory.Instance.AddItemToSlot(index, item, number);
    }

    public override InventoryItemSO GetItem()
    {
        return GroundInventory.Instance.GetItemInSlot(index);
    }

    public override int GetNumber()
    {
        return GroundInventory.Instance.GetNumberInSlot(index);
    }

    public override int MaxAcceptable(InventoryItemSO item)
    {
        if (GroundInventory.Instance.HasSpaceFor(item))
        {
            return int.MaxValue;
        }
        return 0;
    }

    public override void RemoveItems(int number)
    {
        GroundInventory.Instance.RemoveFromSlot(index, number);
    }
}
