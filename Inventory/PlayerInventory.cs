using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : InventoryTetris
{
    public static PlayerInventory Instance;
    public event Action<PlayerInventory> OnPlayerInventoryUpdated;

    public ItemTetrisSO akItem;
    public ItemTetrisSO grenadeItem;
    public ItemTetrisSO moneyItem;
    public ItemTetrisSO medkitItem;

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

        itemContainer.gameObject.SetActive(false);

        TryPlaceItem(akItem, new Vector2Int(3, 1));
        TryPlaceItem(grenadeItem, new Vector2Int(6, 3));
        TryPlaceItem(moneyItem, new Vector2Int(6, 10));
        TryPlaceItem(medkitItem, new Vector2Int(3, 10));

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
        itemContainer.gameObject.SetActive(true);
    }

    private void GearStockManager_OnCloseGearStock()
    {
        itemContainer.gameObject.SetActive(false);
    }

    private void InventoryUI_OnOpenPickup(InventoryUI arg1, OnOpenPickupEventArgs arg2)
    {
        itemContainer.gameObject.SetActive(true);
    }
    private void InventoryUI_OnClosePickup(InventoryUI arg1, OnClosePickupEventArgs arg2)
    {
        itemContainer.gameObject.SetActive(false);
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
