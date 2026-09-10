using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class HousingGrid : MonoBehaviour
{
    [SerializeField] private float cellSize = 1f;
    [SerializeField] public int minGridX = 0;
    [SerializeField] public int minGridY = 0;
    [SerializeField] public int maxGridX = 10;
    [SerializeField] public int maxGridY = 10;

    private Dictionary<Vector2Int, GameObject> occupiedCells = new();

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

            if(cell.x < minGridX || cell.x >= maxGridX || cell.y < minGridY || cell.y >= maxGridY || !IsCellAvailable(cell))
                return false;
        }
        return true;
    }

    public void Place(Vector2Int pos, GridObject obj)
    {
        if(!CanPlace(pos, obj))
            return;

        GameObject placedObject = Instantiate(obj.objPrefabs, GridToWorld(pos), Quaternion.identity);

        List<Vector2Int> cells = GetOccupiedCells(pos, obj);

        foreach(Vector2Int cell in cells)
        {
            occupiedCells.Add(cell, placedObject);
        }
    }
}
