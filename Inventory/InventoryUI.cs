using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI inventorySlotUIPrefab;

    private void Start()
    {
        ReDraw();
    }

    private void OnEnable()
    {
        //Subscribe to on inventory update event
        PlayerInventory.Instance.OnInventoryUpdated += Instance_OnInventoryUpdated;
    }

    private void OnDisable()
    {
        //Unsubscribe from on inventory update event
        PlayerInventory.Instance.OnInventoryUpdated -= Instance_OnInventoryUpdated;
    }
    private void Instance_OnInventoryUpdated(PlayerInventory inventory)
    {
        ReDraw();
    }
    private void ReDraw()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for(int i = 0; i <PlayerInventory.Instance.GetInventorySize(); i++)
        {
            var itemUI = Instantiate(inventorySlotUIPrefab, transform);
            itemUI.Setup(i);
        }
        
    }
}

