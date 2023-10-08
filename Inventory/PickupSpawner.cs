using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// Spawns pickups that should exist on the first load in a level. This
// automatically spawns the correct prefab for a given inventory item.
// </summary>

public class PickupSpawner : MonoBehaviour, ISaveable
{
    [SerializeField] private InventoryItemSO itemSO = null;
    [SerializeField] private int number = 1;

    private void Awake()
    {
        //Spawn in Awake so can be destroyed by save system after.
        SpawnPickup();
    }
   
    // <summary>
    // Spawn pick up object
    // </summary>
    private void SpawnPickup()
    {
        Pickup spawnedPickup = itemSO.SpawnPickup(transform.position, number);
        spawnedPickup.transform.SetParent(transform);
    }

    // <summary>
    // Destroy pick up object.
    // </summary>
    private void DestroyPickup()
    {
        if (GetPickup())
        {
            Destroy(GetPickup().gameObject);
        }
    }

    // <summary>
    // Returns the pickup spawned by this class if it exists.
    // </summary>
    public Pickup GetPickup()
    {
        return GetComponentInChildren<Pickup>();
    }

    // <summary>
    // True if the pickup was collected.
    // </summary>
    public bool isCollected()
    {
        return GetPickup() == null;
    }
    public object CaptureState()
    {
        return isCollected();
    }

    public void RestoreState(object state)
    {
        bool shouldBeCollected = (bool)state;
        if(shouldBeCollected && !isCollected())
        {
            DestroyPickup();
        }
        if(!shouldBeCollected && isCollected())
        {
            SpawnPickup();
        }
    }


}
