using System.Collections.Generic;
using UnityEngine;

public class HousingGrid : MonoBehaviour
{

    [Header("격자 크기")]
    [SerializeField] private float cellSize = 1f;
    [SerializeField] public int minGridX = -18;
    [SerializeField] public int minGridY = -8;
    [SerializeField] public int maxGridX = 18;
    [SerializeField] public int maxGridY = 4;

    [SerializeField] private GridObjectData gridObjectData;

    private List<GameObject> placedObjects = new();
    private Dictionary<Vector2Int, GameObject> occupiedCells = new();

    private void Start()
    {
        //테스트
        // Place(new Vector2Int(-3,-4), gridObjectData.objects[0]);
        // Place(new Vector2Int(-3,-4), gridObjectData.objects[0]);
        LoadPlacedObjects();
    }
    #region 그리드그리기
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

    private void OnDrawGizmos()
    {
       for(int y = minGridY; y < maxGridY; y++)
       {
            Gizmos.DrawLine(new Vector3(minGridX * cellSize, y * cellSize, 0), new Vector3(maxGridX * cellSize, y * cellSize, 0));            
       }
       for (int x = minGridX; x < maxGridX; x++)
       { 
           Gizmos.DrawLine(new Vector3(x * cellSize, minGridY * cellSize, 0), new Vector3(x * cellSize, maxGridY * cellSize, 0));
       }
    }
    #endregion
    
    public bool IsCellAvailable(Vector2Int gridPosition)
    {

        return !occupiedCells.ContainsKey(gridPosition);
    }

    public List<Vector2Int> GetOccupiedCells(Vector2Int pos, GridObject obj)
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

        List<Vector2Int> cells = GetOccupiedCells(pos, obj);

        foreach(Vector2Int cell in cells)
        {
            Debug.Log($"검사 중: {cell}");

            if (cell.x < minGridX || cell.x >= maxGridX || cell.y < minGridY || cell.y >= maxGridY)
            {
                Debug.Log($"맵 범위 밖: {cell}");
                return false;
            }

            if (!IsCellAvailable(cell))
            {
                Debug.Log($"이미 점유됨: {cell}");
                return false;
            }
        }
        return true;
    }

    public void Place(Vector2Int pos, GridObject obj)
    {
        Debug.Log("Place 호출됨");

        if (!CanPlace(pos, obj))
        {
            Debug.Log("배치 불가능");
            return;
        }

        GameObject placedObject = Instantiate(obj.objPrefabs, GridToWorld(pos), Quaternion.identity);

        List<Vector2Int> cells = GetOccupiedCells(pos, obj);

        foreach(Vector2Int cell in cells)
        {
            occupiedCells.Add(cell, placedObject);
        }
    }

    public void Remove(GameObject target)
    {
        List<Vector2Int> cellsToRemove = new();

        foreach (var pair in occupiedCells)
        {
            if (pair.Value == target)
            {
                cellsToRemove.Add(pair.Key);
            }
        }

        if (cellsToRemove.Count > 0)
        {
            foreach (Vector2Int cell in cellsToRemove)
            {
                occupiedCells.Remove(cell);
            }
            Destroy(target);
        }
    }
    public void LoadPlacedObjects()
    {
        foreach (var save in SaveManager.Instance.CurrentData.PlacedObjects)
        {
            var objData = gridObjectData.objects.Find(o => o.objID == save.objId);
            if (objData == null) continue;

            Vector2Int pos = new Vector2Int(save.gridX, save.gridY);
            SpawnAndRegister(pos, objData, save.instanceId);
        }
    }

    private void SpawnAndRegister(Vector2Int pos, GridObject obj, string instanceId)
    {
        GameObject placedObject = Instantiate(obj.objPrefabs, GridToWorld(pos), Quaternion.identity);
        placedObject.AddComponent<PlacedGridObject>().Init(obj.objID, instanceId);

        if (obj.objID == "table")
            TableRegistry.Instance.Register(instanceId, placedObject.transform);

        List<Vector2Int> cells = GetOccupiedCells(pos, obj);
        foreach (Vector2Int cell in cells)
            occupiedCells[cell] = placedObject;

        placedObjects.Add(placedObject);
    }
    public int GetPlacedCount(string objID)
    {
        placedObjects.RemoveAll(o => o == null);
        int count = 0;
        foreach (var obj in placedObjects)
            if (obj.GetComponent<PlacedGridObject>().ObjId == objID) count++;
        return count;
    }
}
