using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// A ScriptableObject that represents any item that can be put in an inventory.
// </summary>
[CreateAssetMenu(fileName = "InvetoryItemSO_",menuName = "Scriptable Objects/Inventory/InventoryItemSO")]
public class InventoryItemSO : ScriptableObject, ISerializationCallbackReceiver
{
    // Config data
    #region Tooltip
    [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
    #endregion
    [SerializeField] private string itemID = null;
    #region Tooltip
    [Tooltip("Item name to be displayed in UI.")]
    #endregion
    [SerializeField] private string displayName = null;
    #region Tooltip
    [Tooltip("Item description to be displayed in UI")]
    #endregion
    [SerializeField] [TextArea] private string description = null;
    #region Tooltip
    [Tooltip("The UI icon to represent this item in the inventory.")]
    #endregion
    [SerializeField] private Sprite icon = null;
    #region Tooltip
    [Tooltip("The prefab that should be spawned when this item is dropped.")]
    #endregion
    [SerializeField] private Pickup pickup = null;
    #region Tooltip
    [Tooltip("If true, multiple item of this type can be stacked in the same inventory slot.")]
    #endregion
    [SerializeField] private bool stackable = false;

    // STATE
    private static Dictionary<string, InventoryItemSO> itemLookupCache;

    // PUBLIC

    // <summary>
    // Get the inventory item instance from its UUID. String UUID that persists between game instances.
    // </summary>
    public static InventoryItemSO GetFromId(string itemID)
    {
        if(itemLookupCache == null)
        {
            itemLookupCache = new Dictionary<string, InventoryItemSO>();
            var itemList = Resources.LoadAll<InventoryItemSO>(""); 
            foreach(var item in itemList)
            {
                if (itemLookupCache.ContainsKey(item.itemID))
                {
                    continue;
                }
                itemLookupCache[item.itemID] = item;
            }
        }
        if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
        return itemLookupCache[itemID];
    }

    // <summary>
    // Spawn the pickup gameobject into the world.
    // </summary>
    public Pickup SpawnPickup(Vector3 position,int number)
    {
        var pickup = Instantiate(this.pickup);
        pickup.transform.position = position;
        pickup.SetUp(this, number);
        return pickup;
    }

    // <summary>
    // Get Icon
    // </summary>
    public Sprite GetIcon()
    {
        return icon;
    }

    // <summary>
    // GetItemID. Returns UUID.
    // </summary>
    public string GetItemID()
    {
        return itemID;
    }

    // <summary>
    // Is Stackable
    // </summary>
    public bool IsStackable()
    {
        return stackable;
    }

    // <summary>
    // Get the name of the item
    // </summary>
    public string GetDisplayName()
    {
        return displayName;
    }

    // <summary>
    // Get the description of the item
    // </summary>
    public string GetDescription()
    {
        return description;
    }


    public void OnAfterDeserialize()
    {
        //Generate and save a new UUID if this is blank.
        if (string.IsNullOrWhiteSpace(itemID))
        {
            itemID = System.Guid.NewGuid().ToString();
        }
    }

    public void OnBeforeSerialize()
    {
        // Require by the ISerializationCallbackReceiver but we don't need
        // to do anything with it
       
    }

}
