using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionItemSO_", menuName = "Scriptable Objects/Inventory/ActionItemSO")]
public class ActionItemSO : InventoryItemSO
{
    #region Tooltip
    [Tooltip("Does an instance of this item get consumed every time it's used")]
    #endregion
    [SerializeField] private bool consumable = false;

    // <summary>
    // Trigger the use of this item. Override to provide functionalty.
    // </summary>
    public virtual void Use(GameObject user)
    {

    }
    public bool IsConsumable()
    {
        return consumable;
    }
}
