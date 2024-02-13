using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : InventoryTetris
{

    public bool isCreated;
    private void Awake()
    {
        int gridWidth = 8;
        int gridHeight = 12;
        float cellSize = 16f;
        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

    }

    public void LoadInventory(string capturedState)
    {
        Load(capturedState);
    }
    public void DestroyPickup()
    {
        Destroy(gameObject);
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
