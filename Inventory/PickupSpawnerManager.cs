using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public class PickupSpawnerManager : MonoBehaviour
{
    [SerializeField] private List<PickupSpawner> pickupSpawnerList;
    private void OnEnable()
    {
        GroundInventory.Instance.OnCreatePickup += GroundInventory_OnGroundInventoryClosed;
    }
    private void OnDisable()
    {
        GroundInventory.Instance.OnCreatePickup -= GroundInventory_OnGroundInventoryClosed;

    }
    private void GroundInventory_OnGroundInventoryClosed(GroundInventory grbInventory, OnGroundInventoryClosedEventArgs groundInventoryClosedEventArgs)
    {
        SpawnPickupSpawner(groundInventoryClosedEventArgs);
    }

    private void SpawnPickupSpawner(OnGroundInventoryClosedEventArgs groundInventoryClosedEventArgs)
    {
        Addressables.LoadAssetAsync<GameObject>(Settings.pickupSpawnerRef).Completed += (asyncOperationHandle) =>
        {
            GameObject pickupSpawner = Instantiate(asyncOperationHandle.Result, groundInventoryClosedEventArgs.position, Quaternion.identity);
            try
            {
                pickupSpawner.GetComponent<PickupSpawner>().LoadInventory(groundInventoryClosedEventArgs.grbInventoryState);
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
        };
    }

    public void DestroyAllPickups()
    {
        foreach(PickupSpawner pickupSpawner in pickupSpawnerList)
        {
            pickupSpawner.DestroyPickup();
        }
        pickupSpawnerList.Clear();
    }

    
}
