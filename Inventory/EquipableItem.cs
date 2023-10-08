using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// An inventory item that can be equipped to the player.
// </summary>

[CreateAssetMenu(fileName = "EquipableItem_",menuName = "Scriptable Objects/Inventory/EquipableItemSO")]
public class EquipableItem : InventoryItemSO
{

    #region Tooltip
    [Tooltip("Where are we allowed to put this item.")]
    #endregion
    [SerializeField] private EquipLocation allowedEquipLocation = EquipLocation.FirstGun;

    public EquipLocation GetAllowedEquipLocation()
    {
        return allowedEquipLocation;
    }
}
