using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public class PickupSpawnerManager : MonoBehaviour
{
    [SerializeField] private List<PickupSpawner> pickupSpawnerList;
    private void Start()
    {
        GroundInventory.Instance.OnCreatePickup += GroundInventory_OnGroundInventoryClosed;
    }
    private void OnDestroy()
    {
        GroundInventory.Instance.OnCreatePickup -= GroundInventory_OnGroundInventoryClosed;

    }
    private void GroundInventory_OnGroundInventoryClosed(GroundInventory groundInventory, GroundInventoryEventArgs groundInventoryEventArgs)
    {
        SpawnPickupSpawner(groundInventoryEventArgs);
    }

    private void SpawnPickupSpawner(GroundInventoryEventArgs groundInventoryEventArgs)
    {
        Addressables.LoadAssetAsync<GameObject>(Settings.pickupSpawnerRef).Completed += (asyncOperationHandle) =>
        {
            GameObject pickupSpawner = Instantiate(asyncOperationHandle.Result, GameManager.Instance.GetPlayer().GetPlayerPosition(), Quaternion.identity);
            try
            {
                if(pickupSpawner != null)
                {
                    pickupSpawner.GetComponent<PickupSpawner>().LoadInventory(groundInventoryEventArgs.pickUpInventoryState);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        };
    }

    public void DestroyAllPickups()
    {
        foreach (PickupSpawner pickupSpawner in pickupSpawnerList)
        {
            pickupSpawner.DestroyPickup();
        }
        pickupSpawnerList.Clear();
    }


}
