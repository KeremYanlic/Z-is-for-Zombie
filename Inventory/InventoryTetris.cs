using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTetris : SingletonMonobehaviour<InventoryTetris> 
{
    public event EventHandler<PlacedObject> OnObjectPlaced;

    [SerializeField] private bool isGroundInventory;
    public event Action<InventoryTetris> OnInventoryTetrisUpdated;
    public event Action<InventoryTetris, InventoryTetrisEventArgs> OnCreatePickup;



    private Grid<GridObject> grid;
    private RectTransform itemContainer;


    protected override void Awake()
    {
        LaunchInventoryTetris();
    }
    private void Start()
    {
        if (!isGroundInventory) return;
        SubscribeToEvents();
    }
    private void OnDestroy()
    {
        if (!isGroundInventory) return;
        UnsubscribeFromEvents();
    }
    private void LaunchInventoryTetris()
    {
        int gridWidth = 8;
        int gridHeight = 12;
        float cellSize = 16f;
        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

        itemContainer = transform.Find("ItemContainer").GetComponent<RectTransform>();

        transform.Find("BackgroundTempVisual").gameObject.SetActive(false);
    }
    private void SubscribeToEvents()
    {
        ShowHideUI.Instance.OnOpenInventory += OnOpenInventory;
        ShowHideUI.Instance.OnCloseInventory += OnCloseInventory;
        ShowHideUI.Instance.OnOpenPickup += OnOpenPickup;
        ShowHideUI.Instance.OnClosePickup += OnClosePickup;
    }
    private void UnsubscribeFromEvents()
    {
        ShowHideUI.Instance.OnOpenInventory -= OnOpenInventory;
        ShowHideUI.Instance.OnCloseInventory -= OnCloseInventory;
        ShowHideUI.Instance.OnOpenPickup -= OnOpenPickup;
        ShowHideUI.Instance.OnClosePickup -= OnClosePickup;
    }


    private void OnOpenInventory()
    {
        OnInventoryTetrisUpdated?.Invoke(this);
    }
    private void OnCloseInventory()
    {
        if (IsThereItem() == false) return;

        CreatePickupEvent(transform.position, Save());
        ResetInventory();
        Save();
        OnInventoryTetrisUpdated?.Invoke(this);
    }
    private void OnOpenPickup(ShowHideUI showHideUI, OnOpenPickupEventArgs onOpenPickupEventArgs)
    {
        string pickupInventoryState = onOpenPickupEventArgs.pickupSpawner.Save();
        Load(pickupInventoryState);

        OnInventoryTetrisUpdated?.Invoke(this);
    }

    private void OnClosePickup(ShowHideUI showHideUI, OnClosePickupEventArgs onClosePickupEventArgs)
    {
        PickupSpawner pickupSpawner = onClosePickupEventArgs.pickupSpawner;

        if (IsThereItem() == false)
        {
            pickupSpawner.DestroyPickup();
            return;
        }

        string pickupLastState = Save();
        pickupSpawner.LoadInventory(pickupLastState);

        ResetInventory();
        OnInventoryTetrisUpdated?.Invoke(this);
    }

    private void CreatePickupEvent(Vector3 pickupPos, string pickupInventoryState)
    {
        OnCreatePickup?.Invoke(this, new InventoryTetrisEventArgs()
        {
            position = pickupPos,
            grbInventoryState = pickupInventoryState
        });
    }
    public bool TryPlaceItem(ItemTetrisSO itemTetrisSO, Vector2Int placedObjectOrigin)
    {
        // Test Can Build
        List<Vector2Int> gridPositionList = itemTetrisSO.GetGridPositionList(placedObjectOrigin);
        bool canPlace = true;
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            bool isValidPosition = grid.IsValidGridPosition(gridPosition);
            if (!isValidPosition)
            {
                // Not valid
                canPlace = false;
                break;
            }
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
            {
                canPlace = false;
                break;
            }
        }

        if (canPlace)
        {
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canPlace = false;
                    break;
                }
            }
        }

        if (canPlace)
        {
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y);

            PlacedObject placedObject = PlacedObject.CreateCanvas(itemContainer, placedObjectWorldPosition, placedObjectOrigin, itemTetrisSO);

            placedObject.GetComponent<InventoryTetrisDragDrop>().Setup(this);

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }

            OnObjectPlaced?.Invoke(this, placedObject);

            // Object Placed!
            return true;
        }
        else
        {
            // Object CANNOT be placed!
            return false;
        }
    }

    public void RemoveItemAt(Vector2Int removeGridPosition)
    {
        PlacedObject placedObject = grid.GetGridObject(removeGridPosition.x, removeGridPosition.y).GetPlacedObject();

        if (placedObject != null)
        {
            // Demolish
            placedObject.DestroySelf();

            List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
            }
        }
    }
    public Grid<GridObject> GetGrid() {
        return grid;
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXY(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public bool IsValidGridPosition(Vector2Int gridPosition) {
        return grid.IsValidGridPosition(gridPosition);
    }


   
    private void ResetInventory()
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

    public bool IsThereItem()
    {
        return itemContainer.childCount > 0;
    }

    public RectTransform GetItemContainer() {
        return itemContainer;
    }
    

    [Serializable]
    public struct AddItemTetris {
        public string itemTetrisSOName;
        public Vector2Int gridPosition;
    }

    [Serializable]
    public struct ListAddItemTetris {
        public List<AddItemTetris> addItemTetrisList;
    }

    public string Save() {
        List<PlacedObject> placedObjectList = new List<PlacedObject>();
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                if (grid.GetGridObject(x, y).HasPlacedObject()) {
                    placedObjectList.Remove(grid.GetGridObject(x, y).GetPlacedObject());
                    placedObjectList.Add(grid.GetGridObject(x, y).GetPlacedObject());
                }
            }
        }

        List<AddItemTetris> addItemTetrisList = new List<AddItemTetris>();
        foreach (PlacedObject placedObject in placedObjectList) {
            addItemTetrisList.Add(new AddItemTetris {
                gridPosition = placedObject.GetGridPosition(),
                itemTetrisSOName = (placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO).name,
            });

        }

        return JsonUtility.ToJson(new ListAddItemTetris { addItemTetrisList = addItemTetrisList });
    }

    public void Load(string loadString) {
        ListAddItemTetris listAddItemTetris = JsonUtility.FromJson<ListAddItemTetris>(loadString);

        foreach (AddItemTetris addItemTetris in listAddItemTetris.addItemTetrisList) {
            TryPlaceItem(InventoryTetrisAssets.Instance.GetItemTetrisSOFromName(addItemTetris.itemTetrisSOName), addItemTetris.gridPosition);
        }
    }

}
public class InventoryTetrisEventArgs : EventArgs
{
    public Vector3 position;
    public string grbInventoryState;
}

