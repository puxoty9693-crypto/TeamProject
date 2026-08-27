using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int gold;

    public List<string> unlockedRecipeIds;
    public List<string> unlockedIngredientIds;
    public List<IngredientStock> warehouseStock;

    public int carriageUpgradeLevel;
    public int adUpgradeLevel;

    public List<NPCUpgradeSave> nPCUpgradeLevels;
}

