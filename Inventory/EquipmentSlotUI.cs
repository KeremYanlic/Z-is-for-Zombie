using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// <summary>
// An slot for the players equipment.
// </summary>
public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItemSO>
{
    [SerializeField] private InventoryItemIcon icon = null;
    [SerializeField] private EquipLocation equipLocation = EquipLocation.FirstGun;

    private Equipment playerEquipment;

    private void Awake()
    {
        playerEquipment = GetComponent<Equipment>();
    }
    private void OnEnable()
    {
        //Subscribe to equipment updated event
        playerEquipment.OnEquipmentUpdated += PlayerEquipment_OnEquipmentUpdated;
    }

    private void OnDisable()
    {
        //Unsubscribe from equipment updated event
        playerEquipment.OnEquipmentUpdated -= PlayerEquipment_OnEquipmentUpdated;
    }
    private void Start()
    {
        PlayerEquipment_OnEquipmentUpdated(playerEquipment);
    }
    private void PlayerEquipment_OnEquipmentUpdated(Equipment equipment)
    {
        icon.SetItem(playerEquipment.GetItemInSlot(equipLocation));
    }

    public int MaxAcceptable(InventoryItemSO itemSO)
    {
        EquipableItem equipableItem = itemSO as EquipableItem;
        if (equipableItem == null) return 0;
        if (equipableItem.GetAllowedEquipLocation() != equipLocation) return 0;
        if (GetItem() != null) return 0;

        return 1;
    }
    // <summary>
    // Add item to equipable slot
    // </summary>
    public void AddItems(InventoryItemSO item, int number)
    {
        playerEquipment.AddItem(equipLocation, (EquipableItem)item);
    }
    // <summary>
    // Remove item from equipable slot
    // </summary>
    public void RemoveItems(int number)
    {
        playerEquipment.RemoveItem(equipLocation);
    }
    // <summary>
    // Return item inside of the slot
    // </summary>
    public InventoryItemSO GetItem()
    {
        return playerEquipment.GetItemInSlot(equipLocation);
    }
   
    public int GetNumber()
    {
        if(GetItem() != null) 
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
   
}
