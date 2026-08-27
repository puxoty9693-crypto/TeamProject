using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NPCNPCUpgradeLevel 
{
    public int upgradeValue;
    public int upgradeGoldCost;
}


[CreateAssetMenu(fileName = "NPCUpgrade_", menuName = "Data/NPCUpgrade")]
public class NPCUpgradeData : ScriptableObject
{
    public NPCRole role;
    public List<NPCNPCUpgradeLevel> levels;
}
