using System;

public class AdUpgradeSystem
{
    private readonly PlayerData curData;

    public AdUpgradeSystem(PlayerData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        curData = data;
    }

    public int GetCurrentLevel()
    {
        return curData.AdUpgradeLevel;
    }

    public float GetUpgradeValue()
    {
        AdData data = GetUpgradeData();

        if (data == null ||
            data.levels == null ||
            data.levels.Count == 0)
        {
            return 1f;
        }

        int level = GetCurrentLevel();

        if (level < 0 || level >= data.levels.Count)
            return 1f;

        return data.levels[level].upgradeValue;
    }

    public float GetCustomerSpawnDelay(float originDelay)
    {
        if (originDelay <= 0f)
            return 0f;

        float value = GetUpgradeValue();

        if (value <= 0f) return originDelay;

        return originDelay / value;
    }

    public bool CanUpgrade()
    {
        AdData data = GetUpgradeData();

        if (!TryGetNextLevel(data, out int nextLevel))
            return false;

        return curData.Gold >= data.levels[nextLevel].UpgradeGoldCost;
    }

    public bool Upgrade()
    {
        AdData data = GetUpgradeData();

        if (!TryGetNextLevel(data, out int nextLevel))
            return false;

        int cost = data.levels[nextLevel].UpgradeGoldCost;

        if (!curData.SpendGold(cost))
            return false;

        curData.UpgradeAdLevel();

        return true;
    }

    public int GetNextUpgradeCost()
    {
        AdData data = GetUpgradeData();

        if (!TryGetNextLevel(data, out int nextLevel))
            return -1;

        return data.levels[nextLevel].UpgradeGoldCost;
    }

    private AdData GetUpgradeData()
    {
        if (DataManager.Instance == null)
            return null;

        return DataManager.Instance.adData;
    }

    private bool TryGetNextLevel(
        AdData data,
        out int nextLevel)
    {
        nextLevel = -1;

        if (data == null ||
            data.levels == null)
        {
            return false;
        }

        int currentLevel = GetCurrentLevel();

        nextLevel = currentLevel + 1;

        return nextLevel >= 0 && nextLevel < data.levels.Count;
    }
}