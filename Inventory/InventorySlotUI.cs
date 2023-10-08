using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItemSO>
{
    [SerializeField] private InventoryItemIcon icon = null;

    private int index;
    private InventoryItemSO itemSO;
    private Inventory inventory;
    public void SetUp(Inventory inventory, int index)
    {
        this.inventory = inventory;
        this.index = index;

        icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
    }

    public int MaxAcceptable(InventoryItemSO item)
    {
        if (inventory.HasSpaceFor(item))
        {
            return int.MaxValue;
        }
        return 0;
    }
    public void AddItems(InventoryItemSO item, int number)
    {
        inventory.AddItemToSlot(index, item, number);
    }

    public InventoryItemSO GetItem()
    {
        return inventory.GetItemInSlot(index);
    }

    public int GetNumber()
    {
        return inventory.GetNumberInSlot(index);
    }
    public void RemoveItems(int number)
    {
        inventory.RemoveFromSlot(index, number);
    }

  
   
}
