using UnityEngine;

public class TableManager : MMSingleton<TableManager>
{
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

    //특정 테이블 강화
    public bool UpgradeTable(string tableId) 
    {
        var data = DataManager.Instance.tableUpgrades.Find(t => t.tableId == tableId);
        if (data == null) return false;

        int currentLevel = SaveManager.Instance.CurrentData.GetTableLevel(tableId);
        if (currentLevel >= data.levels.Count) return false;

        int cost = data.levels[currentLevel].upgradeGoldCost;
        if (!SaveManager.Instance.CurrentData.SpendGold(cost)) return false;

        SaveManager.Instance.CurrentData.UpgradeTableLevel(tableId);
        return true;
    }

}
