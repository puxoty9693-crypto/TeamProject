using System.Collections.Generic;
using UnityEngine;
// TableRegistry.Instance.GetTable(id) / GetAllTables() 사용으로 테이블 조회
public class TableRegistry : MMSingleton<TableRegistry>
{
    private Dictionary<string, Transform> tables = new();

    public void Register(string tableId, Transform tableTransform)
        => tables[tableId] = tableTransform;

    public void Unregister(string tableId)
        => tables.Remove(tableId);

    public Transform GetTable(string tableId)
        => tables.TryGetValue(tableId, out var t) ? t : null;

    public IReadOnlyDictionary<string, Transform> GetAllTables() => tables;
}