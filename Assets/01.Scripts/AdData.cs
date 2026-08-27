using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AdUpgradeLevel 
{
    public int upgradeGoldCost;
    public string upgradeText;
}



[CreateAssetMenu(fileName = "Ad_", menuName = "Data/Ad")]
public class AdData : ScriptableObject
{
    public List<AdUpgradeLevel> upgradeLevel;
}
