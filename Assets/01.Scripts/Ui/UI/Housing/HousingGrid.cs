using System.Collections.Generic;
using UnityEngine;

public class HousingGrid : MonoBehaviour
{
    #region Grid Settings
    [Header("격자 크기")]
    [SerializeField] private float cellSize = 1f;
    [SerializeField] public int minGridX = -18;
    [SerializeField] public int minGridY = -8;
    [SerializeField] public int maxGridX = 18;
    [SerializeField] public int maxGridY = 4;
    [SerializeField] private GameObject gridCellPrefab;
    #endregion

    #region Grid Data

    [SerializeField] private GridObjectData gridObjectData;

    private Dictionary<Vector2Int, GameObject> gridCells = new();
    private Dictionary<Vector2Int, GameObject> occupiedCells = new();
    private List<GameObject> placedObjects = new();

    #endregion

    #region Holding

    private GameObject heldObject;
    private GridObject heldObjectData;
    private Vector2Int heldOriginalPos;

    public bool IsHolding => heldObject != null;
    public GridObject HeldObjectData => heldObjectData;

    #endregion

    private void Start()
    {
        CreateGrid();
        LoadPlacedObjects();
    }

    #region Grid Coordinate
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / cellSize);
        int y = Mathf.FloorToInt(worldPosition.y / cellSize);

        return new Vector2Int(x, y);
    }
    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        float x = (gridPosition.x + 0.5f) * cellSize;
        float y = (gridPosition.y + 0.5f) * cellSize;

        return new Vector3(x, y, 0f);
    }
    public Vector3 GetFootprintCenter(Vector2Int anchorPos, Vector2Int size)
    {
        float centerX = (anchorPos.x + size.x / 2f) * cellSize;
        float centerY = (anchorPos.y + size.y / 2f) * cellSize;
        return new Vector3(centerX, centerY, 0f);
    }
    public bool IsInGrid(Vector2Int gridPos)
    {
        return gridPos.x >= minGridX && gridPos.x < maxGridX && gridPos.y >= minGridY && gridPos.y < maxGridY;
    }
    #endregion

    #region Grid Cell
    private void CreateGrid()
    {
        for (int y = minGridY; y < maxGridY; y++)
        {
            for (int x = minGridX; x < maxGridX; x++)
            {
                Vector2Int gridPosition = new Vector2Int(x, y);

                GameObject cell = Instantiate(gridCellPrefab, GridToWorld(gridPosition), Quaternion.identity,transform);

                gridCells.Add(gridPosition, cell);
            }
        }
    }
    public GameObject GetCell(Vector2Int gridPosition)
    {
        return gridCells.TryGetValue(gridPosition, out var cell)? cell : null;
    }

    public bool IsCellAvailable(Vector2Int gridPosition)
    {

        return !occupiedCells.ContainsKey(gridPosition);
    }
    public List<Vector2Int> GetObjectCells(Vector2Int pos, GridObject obj)
    {

        Vector2Int size = obj.size;

        List<Vector2Int> cells = new();
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            { 
                int cellX = pos.x + x;
                int cellY = pos.y + y;


                Vector2Int cell = new (cellX,cellY);
                
                cells.Add(cell);
            }
        }
        return cells;
    }
    public bool CanPlace(Vector2Int pos, GridObject obj)
    {

        List<Vector2Int> cells = GetObjectCells(pos, obj);

        foreach(Vector2Int cell in cells)
        {

            if (!IsInGrid(cell))
                return false;

            if (!IsCellAvailable(cell))
                return false;
        }
        return true;
    }
    #endregion

    #region Object Placement
    public void Place(Vector2Int pos, GridObject obj)
    {

        if (!CanPlace(pos, obj))
            return;

        string instanceId = obj.objID == ObjectIds.Table? SaveManager.Instance.CurrentData.GetNextTableId() : System.Guid.NewGuid().ToString();

        if(!SpawnAndRegister(pos,obj,instanceId))
            return;

        SaveManager.Instance.CurrentData.AddPlacedObject(obj.objID, instanceId, pos);
    }
    private bool SpawnAndRegister(Vector2Int pos, GridObject obj, string instanceId)
    {
        GameObject placedObject = Instantiate(obj.objPrefabs, GetFootprintCenter(pos, obj.size), Quaternion.identity);

        PlacedGridObject info = placedObject.GetComponent<PlacedGridObject>();
        if (info == null)
        {
            Destroy(placedObject);
            return false;    
        }

        info.Init(obj.objID, instanceId);

        if (obj.objID == ObjectIds.Table)
            TableRegistry.Instance.Register(instanceId, placedObject.transform);

        foreach (Vector2Int cell in GetObjectCells(pos, obj))
            occupiedCells[cell] = placedObject;

        placedObjects.Add(placedObject);
        
        return true;
    }
    #endregion

    #region Object Removal
    public void Remove(GameObject target)
    {
        if (target == null)
            return;
        
        var info = target.GetComponent<PlacedGridObject>();

        if (info == null)
            return;

        List<Vector2Int> cellsToRemove = GetCellsOccupiedBy(target);

        if (cellsToRemove.Count == 0)
            return;

        foreach (Vector2Int cell in cellsToRemove)
        {
            occupiedCells.Remove(cell);
        }

        placedObjects.Remove(target);


        if (info.ObjId == ObjectIds.Table)
        {
            TableRegistry.Instance.Unregister(info.InstanceId);
        }

        SaveManager.Instance.CurrentData.RemovePlacedObject(info.InstanceId);

        Destroy(target);
        
    }
    public int GetPlacedCount(string objID)
    {
        placedObjects.RemoveAll(o => o == null);
        int count = 0;
        foreach (var obj in placedObjects)
        {
            if (obj.GetComponent<PlacedGridObject>().ObjId == objID) count++;
        }
        return count;
    }
    #endregion

    #region Object Movement
    public bool PickUp(GameObject target)
    {
        if (IsHolding)
            return false;

        var info = target.GetComponent<PlacedGridObject>();

        if (info == null)
            return false;

        var objData = GetGridObject(info.ObjId);

        if (objData == null)
            return false;

        Vector2Int? originalAnchor = FindAnchorOf(target);

        if (originalAnchor == null)
            return false;

        List<Vector2Int> cellsToRemove = GetCellsOccupiedBy(target);

        foreach (var cell in cellsToRemove)
            occupiedCells.Remove(cell);


        heldObject = target;
        heldObjectData = objData;
        heldOriginalPos = originalAnchor.Value;

        return true;
    }

    private Vector2Int? FindAnchorOf(GameObject target)
    {
        List<Vector2Int> cells = GetCellsOccupiedBy(target);

        if (cells.Count == 0)
            return null;

        int minX = int.MaxValue;
        int minY = int.MaxValue;

        foreach (var c in cells)
        {
            if (c.x < minX) minX = c.x;
            if (c.y < minY) minY = c.y;
        }

        return new Vector2Int(minX, minY);
    }

    public bool TryDrop(Vector2Int pos)
    {
        if (!IsHolding)
            return false;
        if (!CanPlace(pos, heldObjectData)) 
            return false;

        heldObject.transform.position = GetFootprintCenter(pos, heldObjectData.size);

        List<Vector2Int> cells = GetObjectCells(pos, heldObjectData);

        foreach (var cell in cells)
        {
            occupiedCells[cell] = heldObject;
        }

        var info = heldObject.GetComponent<PlacedGridObject>();
        SaveManager.Instance.CurrentData.UpdatePlacedObjectPosition(info.InstanceId, pos);


        heldObject = null;
        heldObjectData = null;
        return true;
    }

    public void CancelPickUp()
    {
        if (!IsHolding)
            return;
        TryDrop(heldOriginalPos);
    }

    #endregion

    #region Object Query
    public GameObject GetObjectAt(Vector2Int cell)
    => occupiedCells.TryGetValue(cell, out var obj) ? obj : null;
    public List<Vector2Int> GetCellsOccupiedBy(GameObject target)
    {
        List<Vector2Int> cells = new();
        foreach (var pair in occupiedCells)
        {
            if (pair.Value == target) cells.Add(pair.Key);
        }

        return cells;
    }

    private GridObject GetGridObject(string objID)
    {
        return gridObjectData.objects.Find(o => o.objID == objID);
    }
    #endregion

    #region Save / Load
    public void LoadPlacedObjects()
    {
        foreach (var save in SaveManager.Instance.CurrentData.PlacedObjects)
        {
            var objData = GetGridObject(save.objId);
            if (objData == null)
                continue;
  
            Vector2Int pos = new Vector2Int(save.gridX, save.gridY);
            SpawnAndRegister(pos, objData, save.instanceId);
        }
    }
    #endregion
}
