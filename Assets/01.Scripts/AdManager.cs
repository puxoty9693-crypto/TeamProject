using UnityEngine;

public class AdManager : MMSingleton<AdManager>
{
    public int GetLevel()
         => SaveManager.Instance.CurrentData.AdUpgradeLevel;

    public bool Upgrade() 
    {
        return UpgradeHelper.TryUpgrade(GetLevel(), DataManager.Instance.adData.levels, () => SaveManager.Instance.CurrentData.UpgradeAdLevel());
    }

    //현재 레벨의 안내 텍스트
    public string GetCurrentUpgradeText() 
    {
        int level = GetLevel();
        var levels = DataManager.Instance.adData.levels;
        return level < levels.Count ? levels[level].upgradeText : "최대 레벨";
    }
}
