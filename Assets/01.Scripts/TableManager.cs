using UnityEngine;

public class TableManager : MMSingleton<TableManager>
{
    private const int CapacityPerTable = 4; // 테이블 1개당 고정 수용 인원

    public int GetLevel() => SaveManager.Instance.CurrentData.TableUpgradeLevel;

    public bool Upgrade()
    {
        //return UpgradeHelper.TryUpgrade(GetLevel(), DataManager.Instance.tableUpgradeData.levels, () => SaveManager.Instance.CurrentData.UpgradeTableLevel());
        return false;
    }

    // 현재 레벨에서 설치 가능한 최대 테이블 개수
    public int GetMaxTableCount()
    {
        int level = GetLevel();
        var levels = DataManager.Instance.tableUpgradeData.levels;
        return level < levels.Count ? levels[level].tableCount : levels[levels.Count - 1].tableCount;
    }

    // 현재 매장의 총 수용 가능 인원
    public int GetTotalCapacity()
    {
        return TableRegistry.Instance.GetAllTables().Count * CapacityPerTable;
    }
}