using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipableItemSO_",menuName = "Scriptable Objects/Inventory/EquipableItemSO")]
public class EquipableItemSO : InventoryItemSO
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
