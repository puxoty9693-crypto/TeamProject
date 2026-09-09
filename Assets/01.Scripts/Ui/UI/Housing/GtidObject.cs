using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridObject
{
    public string objID;
    public Vector2Int size;
    public GameObject objPrefabs;
}


[CreateAssetMenu(fileName = "GridOBJ", menuName = "Data/GridOBJ")]
public class GridObjectData : ScriptableObject
{
    public List<GridObject> objects = new();
}