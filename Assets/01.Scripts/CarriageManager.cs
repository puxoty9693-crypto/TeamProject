using UnityEngine;

public class CarriageManager : MMSingleton<CarriageManager>
{
    public int GetLevel(string ingredientId)
        => SaveManager.Instance.CurrentData.GetCarriageLevel(ingredientId);

    public bool Upgrade(string ingredientId) 
    {
        var data = DataManager.Instance.carriageUpgrades.Find( c => c.ingredient.ingredientId == ingredientId );
        if (data == null ) return false;

        int currentLevel = GetLevel(ingredientId);
        if (currentLevel >= data.levels.Count) return false; //이미 최대 레벨

        int cost = data.levels[currentLevel].upgradeGoldCost;
        if (!SaveManager.Instance.CurrentData.SpendGold(cost)) return false;

        SaveManager.Instance.CurrentData.UpgradeCarriageLevel(ingredientId);
        return true;
    }
}
