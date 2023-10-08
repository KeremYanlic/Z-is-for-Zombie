using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// To be placed at the root of a Pickup prefab. Contains the data about the 
// pickup such as the type of item and the number.
// </summary.

public class Pickup : MonoBehaviour
{
    private InventoryItemSO itemSO;
    private int number = 1;

    // <summary>
    // Set the vital data after creating the prefab.
    // </summary>
    public void SetUp(InventoryItemSO itemSO,int number)
    {
        this.itemSO = itemSO;
        if (!itemSO.IsStackable())
        {
            number = 1;
        }
        this.number = number;
    }
    public InventoryItemSO GetItem()
    {
        return itemSO;
    }
    public int GetNumber()
    {
        return number;
    }

    public void PickupItem()
    {
        bool foundSlot = Inventory.Instance.AddToFirstEmptySlot(itemSO, number);
        if (foundSlot)
        {
            Destroy(gameObject);
        }
    }

    public bool CanBePickUp()
    {
        return Inventory.Instance.HasSpaceFor(itemSO);
    }
}
