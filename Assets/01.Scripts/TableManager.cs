using UnityEngine;

public class TableManager : MMSingleton<TableManager>
{
    public int GetLevel(string tableId)
        => SaveManager.Instance.CurrentData.GetTableLevel(tableId);

    //특정 테이블 강화
    public bool UpgradeTable(string tableId) 
    {
        var data = DataManager.Instance.tableUpgrades.Find(t => t.tableId == tableId);
        if (data == null) return false;

        return UpgradeHelper.TryUpgrade(GetLevel(tableId), data.levels,() => SaveManager.Instance.CurrentData.UpgradeTableLevel(tableId));
    }

    //현재 매장의 총 수용 가능 인원 계산
    public int GetTotallCapacity()
    {
        int total = 0;
        foreach (var tableData in DataManager.Instance.tableUpgrades)
        {
            int level = SaveManager.Instance.CurrentData.GetTableLevel(tableData.tableId);
            if (level < tableData.levels.Count)
                total += tableData.levels[level].capacity;
        }
        return total;
    }

}
