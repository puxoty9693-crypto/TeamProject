using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TakeoutUpgradeLevel
{
    public float takeoutChance;
    public int upgradeGoldCost;

}

[CreateAssetMenu(fileName = "TakeoutUpgrade", menuName = "Data/TakeoutUpgrade")]
public class TakeoutUpgradeData: ScriptableObject
{
    public List<TakeoutUpgradeLevel> levels;
}

