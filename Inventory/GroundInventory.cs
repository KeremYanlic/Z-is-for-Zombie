using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GroundInventory : InventoryTetris
{
    public static GroundInventory Instance;

    public event Action<GroundInventory> OnGroundInventoryUpdated;
    public event Action<GroundInventory, GroundInventoryEventArgs> OnCreatePickup;
    [SerializeField] private InventoryUI inventoryUI;

    private void Awake()
    {
        Instance = this;

        int gridWidth = 8;
        int gridHeight = 12;
        float cellSize = 16f;
        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

        transform.Find("BackgroundTempVisual").gameObject.SetActive(false);

      
    }
    private void Start()
    {
        GearStockManager.Instance.OnOpenGearStock += GearStockManager_OnOpenGearStock;
        GearStockManager.Instance.OnCloseGearStock += GearStockManager_OnCloseGearStock;
        inventoryUI.OnOpenPickup += InventoryUI_OnOpenPickup;
        inventoryUI.OnClosePickup += InventoryUI_OnClosePickup;

    }
    private void OnDestroy()
    {
        GearStockManager.Instance.OnOpenGearStock -= GearStockManager_OnOpenGearStock;
        GearStockManager.Instance.OnCloseGearStock -= GearStockManager_OnCloseGearStock;
        inventoryUI.OnOpenPickup -= InventoryUI_OnOpenPickup;
        inventoryUI.OnClosePickup -= InventoryUI_OnClosePickup;
    }

    private void GearStockManager_OnOpenGearStock()
    {
        OnGroundInventoryUpdated?.Invoke(this);

        itemContainer.gameObject.SetActive(true);
    }
    private void GearStockManager_OnCloseGearStock()
    {
        itemContainer.gameObject.SetActive(false);
        if (IsThereItem() == false) return;

        CreatePickupEvent(Save());
        ResetInventory();
        Save();
        OnGroundInventoryUpdated?.Invoke(this);
    }
    private void InventoryUI_OnOpenPickup(InventoryUI inventoryUI, OnOpenPickupEventArgs onOpenPickupEventArgs)
    {
        itemContainer.gameObject.SetActive(true);
        string pickupInventoryState = onOpenPickupEventArgs.pickupSpawner.Save();
        Load(pickupInventoryState);

        OnGroundInventoryUpdated?.Invoke(this);
    }


    private void InventoryUI_OnClosePickup(InventoryUI inventoryUI, OnClosePickupEventArgs onClosePickupEventArgs)
    {
        itemContainer.gameObject.SetActive(false);

        PickupSpawner pickupSpawner = onClosePickupEventArgs.pickupSpawner;

        if (IsThereItem() == false)
        {
            pickupSpawner.DestroyPickup();
            return;
        }
        string pickupLastState = Save();
        pickupSpawner.LoadInventory(pickupLastState);

        ResetInventory();
        OnGroundInventoryUpdated?.Invoke(this);
    }

    private void CreatePickupEvent(string pickupInventoryState)
    {
        OnCreatePickup?.Invoke(this, new GroundInventoryEventArgs()
        {
            pickUpInventoryState = pickupInventoryState
        });
    }
    internal void ResetInventory()
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                if (grid.GetGridObject(x, y).HasPlacedObject())
                {
                    RemoveItemAt(new Vector2Int(x, y));
                }
            }
        }
    }

    internal bool IsThereItem()
    {
        return itemContainer.childCount > 0;
    }

    [Serializable]
    public struct AddItemTetris
    {
        public string itemTetrisSOName;
        public Vector2Int gridPosition;
    }

    [Serializable]
    public struct ListAddItemTetris
    {
        public List<AddItemTetris> addItemTetrisList;
    }

    public string Save()
    {
        List<PlacedObject> placedObjectList = new List<PlacedObject>();
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                if (grid.GetGridObject(x, y).HasPlacedObject())
                {
                    placedObjectList.Remove(grid.GetGridObject(x, y).GetPlacedObject());
                    placedObjectList.Add(grid.GetGridObject(x, y).GetPlacedObject());
                }
            }
        }

        List<AddItemTetris> addItemTetrisList = new List<AddItemTetris>();
        foreach (PlacedObject placedObject in placedObjectList)
        {
            addItemTetrisList.Add(new AddItemTetris
            {
                gridPosition = placedObject.GetGridPosition(),
                itemTetrisSOName = (placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO).name,
            });

        }

        return JsonUtility.ToJson(new ListAddItemTetris { addItemTetrisList = addItemTetrisList });
    }

    public void Load(string loadString)
    {
        ListAddItemTetris listAddItemTetris = JsonUtility.FromJson<ListAddItemTetris>(loadString);

        foreach (AddItemTetris addItemTetris in listAddItemTetris.addItemTetrisList)
        {
            TryPlaceItem(InventoryTetrisAssets.Instance.GetItemTetrisSOFromName(addItemTetris.itemTetrisSOName), addItemTetris.gridPosition);
        }
    }
}
public class GroundInventoryEventArgs : EventArgs
{
    public string pickUpInventoryState;
}
