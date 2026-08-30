using UnityEngine;

public class TableManager : MMSingleton<TableManager>
{
    //현재 매장의 총 수용 가능 인원 계산
   public int GetTotallCapacity() 
   {
        int total = 0;
        foreach (var save in SaveManager.Instance.CurrentData.tableLevels) 
        {
            var data = DataManager.Instance.tableUpgrades.Find(t => t.tableId == save.tableId);
            if (data != null && save.level < data.levels.Count) 
            {
                total += data.levels[save.level].capacity;
            } 
        }
        return total;
   }

    //특정 테이블 강화
    public bool UpgradeTable(string tableId) 
    {
        var save = SaveManager.Instance.CurrentData.tableLevels.Find(t => t.tableId == tableId);
        var data = DataManager.Instance.tableUpgrades.Find(t => t.tableId == tableId);
        if (save == null || data == null) return false;

        int nextLevel = save.level + 1;
        if (nextLevel >= data.levels.Count) return false;

        int cost = data.levels[save.level].upgradeGoldCost;
        if (SaveManager.Instance.CurrentData.gold < cost) return false;

        SaveManager.Instance.CurrentData.gold -= cost;
        save.level = nextLevel;
        return true;
    }

}
