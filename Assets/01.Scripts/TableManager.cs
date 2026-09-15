using UnityEngine;

public class TableManager : MMSingleton<TableManager>
{
    private const int CapacityPerTable = 4; // 테이블 1개당 고정 수용 인원

    public int GetLevel()
        => SaveManager.Instance.CurrentData.TableUpgradeLevel;

    public bool Upgrade()
    {
        return UpgradeHelper.TryUpgrade(GetLevel(), DataManager.Instance.tableUpgradeData.levels, () => SaveManager.Instance.CurrentData.UpgradeTableLevel());
    }

    public int GetMaxTableCount()
    {
        int level = GetLevel();
        var levels = DataManager.Instance.tableUpgradeData.levels;
        return level < levels.Count ? levels[level].tableCount : levels[levels.Count - 1].tableCount;
    }

    public int GetTotalCapacity()
    {
        return TableRegistry.Instance.GetAllTables().Count * CapacityPerTable;
    }
}