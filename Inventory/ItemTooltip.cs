using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// <summary>
// Root of the tooltip prefab to expose properties to other classes.
// </summary>
public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText = null;
    [SerializeField] private TextMeshProUGUI bodyText = null;

    public void Setup(InventoryItemSO itemSO)
    {
        titleText.text = itemSO.GetDisplayName();
        bodyText.text = itemSO.GetDescription();
    }
}
