using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// To be placed on the root of the inventory UI. Handles spawning all the
// inventory slot prefabs.
// </summary>
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI inventoryItemPrefab = null;

    private void OnEnable()
    {
        //Subscribe to inventory updated event
        Inventory.Instance.OnInventoryUpdated += Inventory_OnInventoryUpdated;
    }
    private void OnDisable()
    {
        //Unsubscribe from inventory updated event
        Inventory.Instance.OnInventoryUpdated -= Inventory_OnInventoryUpdated;
    }
    private void Start()
    {
        Inventory_OnInventoryUpdated(Inventory.Instance);   
    }

    private void Inventory_OnInventoryUpdated(Inventory obj)
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for(int i=0; i < Inventory.Instance.GetSize(); i++)
        {
            InventorySlotUI itemUI = Instantiate(inventoryItemPrefab, transform);
            itemUI.SetUp(Inventory.Instance, i);
        }
    }
}
