using UnityEngine;

public class CarriageManager : MMSingleton<CarriageManager>
{
    // 해금
    public bool IsUnlocked(string ingredientId)
        => SaveManager.Instance.CurrentData.IsIngredientUnlocked(ingredientId);

    public bool UnlockedIngredient(string ingredientId) 
    {
        if (IsUnlocked(ingredientId)) return false; // 이미 해금됨

        var data = DataManager.Instance.carriageUpgrades.Find(c => c.ingredient.ingredientId == ingredientId);
        if (data == null) return false;

        if (!SaveManager.Instance.CurrentData.SpendGold(data.unlockGoldCost)) return false;

        SaveManager.Instance.CurrentData.UnlockIngredient(ingredientId);
        return true;
    }



    // 강화
    public int GetLevel(string ingredientId)
        => SaveManager.Instance.CurrentData.GetCarriageLevel(ingredientId);

    public bool Upgrade(string ingredientId) 
    {
        if (!IsUnlocked(ingredientId))return false;

        var data = DataManager.Instance.carriageUpgrades.Find( c => c.ingredient.ingredientId == ingredientId );
        if (data == null ) return false;

        return UpgradeHelper.TryUpgrade(GetLevel(ingredientId), data.levels, () => SaveManager.Instance.CurrentData.UpgradeCarriageLevel(ingredientId));
    }
}
