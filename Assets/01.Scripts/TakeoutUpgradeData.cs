using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TakeoutUpgradeLevel : IUpgradeLevel
{
    public float takeoutChance;
    public int upgradeGoldCost;
    public int UpgradeGoldCost => upgradeGoldCost;
}

[CreateAssetMenu(fileName = "TakeoutUpgrade", menuName = "Data/TakeoutUpgrade")]
public class TakeoutUpgradeData: ScriptableObject
{
    public List<TakeoutUpgradeLevel> levels;
}

