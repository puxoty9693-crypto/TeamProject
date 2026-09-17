using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridObject
{
    public string objID;
    public Vector2Int size;
    public GameObject objPrefabs;
    public Sprite icon;
    public int maxCount = -1;
}


[CreateAssetMenu(fileName = "GridOBJ", menuName = "Data/GridOBJ")]
public class GridObjectData : ScriptableObject
{
    public List<GridObject> objects = new();
}