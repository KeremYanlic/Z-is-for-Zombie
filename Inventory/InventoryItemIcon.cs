using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//<summary>
// To be put on the icon representing an inventory item. Allows the slot to
// update the icon and number.
//</summary>
[RequireComponent(typeof(Image))]
[DisallowMultipleComponent]
public class InventoryItemIcon : MonoBehaviour
{
    [SerializeField] private GameObject textContainer = null;
    [SerializeField] private TextMeshProUGUI itemNumber = null;

    public void SetItem(InventoryItemSO itemSO)
    {
        SetItem(itemSO, 0);
    }
    public void SetItem(InventoryItemSO itemSO,int number)
    {
        Image iconImage = GetComponent<Image>();
        if(itemSO == null)
        {
            iconImage.enabled = false;
        }
        else
        {
            iconImage.enabled = true;
            iconImage.sprite = itemSO.GetIcon();
        }

        if (itemNumber)
        {
            if(number <= 1)
            {
                textContainer.SetActive(false);
            }
            else
            {
                textContainer.SetActive(true);
                itemNumber.text = number.ToString();
            }
        }
    }
}
