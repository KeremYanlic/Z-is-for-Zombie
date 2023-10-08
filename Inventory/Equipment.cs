using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// <summary>
// Provides a store for the items equipped to a player. Items are stored by their equip locations.
// </summary>


public class Equipment : MonoBehaviour,ISaveable
{
    //Broadcast when the items in the slots are added/removed.
    public event Action<Equipment> OnEquipmentUpdated;

    Dictionary<EquipLocation, EquipableItem> equippedItemsDictionary = new Dictionary<EquipLocation, EquipableItem>(); 
  

    // <summary>
    // Return the item in the given equip location.
    // </summary>
    public EquipableItem GetItemInSlot(EquipLocation equipLocation)
    {
        if(!equippedItemsDictionary.ContainsKey(equipLocation) || !equippedItemsDictionary[equipLocation])
        {
            return null;
        }
        return equippedItemsDictionary[equipLocation];
    }

    // <summary>
    // Add an item to the given equip location. Do not attempt to equip to an incompatible slot.
    // </summary>
    public void AddItem(EquipLocation equipLocation,EquipableItem equipableItem)
    {
        equippedItemsDictionary[equipLocation] = equipableItem;

        OnEquipmentUpdated?.Invoke(this);
    }

    // <summary>
    // Remove the item for the given slot
    // </summary>
    public void RemoveItem(EquipLocation equipLocation)
    {
        equippedItemsDictionary.Remove(equipLocation);

        OnEquipmentUpdated?.Invoke(this);
    }


    public object CaptureState()
    {
        Dictionary<EquipLocation, string> equippedItemsForSerialization = new Dictionary<EquipLocation, string>();
        foreach (KeyValuePair<EquipLocation,EquipableItem> pair in equippedItemsDictionary)
        {
            equippedItemsForSerialization[pair.Key] = pair.Value.GetItemID(); 
        }
        return equippedItemsForSerialization;
    }

    public void RestoreState(object state)
    {
        equippedItemsDictionary = new Dictionary<EquipLocation, EquipableItem>();

        Dictionary<EquipLocation, string> equippedItemsForSerialization = (Dictionary<EquipLocation, string>)state;

        foreach(KeyValuePair<EquipLocation,string> pair in equippedItemsForSerialization)
        {
            EquipableItem item = (EquipableItem)InventoryItemSO.GetFromId(pair.Value);
            if(item != null)
            {
                equippedItemsDictionary[pair.Key] = item;
            }
        }
    }

}
