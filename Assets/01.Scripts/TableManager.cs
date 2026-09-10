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

    // 현재 레벨에서 설치 가능한 최대 테이블 개수
    public int GetMaxTableCount()
    {
        int level = GetLevel();
        var levels = DataManager.Instance.tableUpgradeData.levels;
        return level < levels.Count ? levels[level].tableCount : levels[levels.Count - 1].tableCount;
    }

    // 현재 매장의 총 수용 가능 인원
    // TODO(하우징 연동): 실제로 "배치된" 테이블 개수를 받아와야 정확함.
    // 하우징 시스템 완성 전까지는 해금된 만큼 전부 배치했다고 가정
    public int GetTotalCapacity()
    {
        return GetMaxTableCount() * CapacityPerTable;
    }
}