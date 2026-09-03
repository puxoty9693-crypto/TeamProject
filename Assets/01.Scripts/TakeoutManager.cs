using UnityEngine;

public class TakeoutManager : MMSingleton<TakeoutManager>
{
    public int GetLevel()
        => SaveManager.Instance.CurrentData.TakeoutUpgradeLevel;

    public bool Upgrade() 
    {
        return UpgradeHelper.TryUpgrade(GetLevel(), DataManager.Instance.takeoutUpgradeData.levels, () => SaveManager.Instance.CurrentData.UpgradeTakeoutLevel());
    }

    public float GetCurrentTakeOutChance() 
    {
        int level = GetLevel();
        var levels = DataManager.Instance.takeoutUpgradeData.levels;
        return level < levels.Count ? levels[level].takeoutChance : levels[levels.Count - 1].takeoutChance;
    }

}
