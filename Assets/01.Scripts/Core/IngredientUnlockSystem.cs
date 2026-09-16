using System;

public class IngredientUnlockSystem
{
    private readonly PlayerData curData;

    public event Action<IngredientData> OnIngredientUnlocked;

    public IngredientUnlockSystem(PlayerData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        curData = data;
    }

    public bool CanUnlock(IngredientData ingredient)
    {
        if (ingredient == null)
            return false;

        if (string.IsNullOrEmpty(ingredient.ingredientId))
            return false;

        // 이미 해금되어 있으면 해금 불가
        if (curData.IsIngredientUnlocked(ingredient.ingredientId))
            return false;

        //임시 로드 추후 수정해야함
        CarriageIngredientData cIData =
        DataManager.Instance.carriageUpgrades.Find(
            x => x != null &&
                x.ingredient != null &&
                x.ingredient.ingredientId == ingredient.ingredientId
        );
        if (cIData == null)
            return false;
        if (curData.Gold < cIData.unlockGoldCost)
            return false;

        return true;
    }

    public bool Unlock(IngredientData ingredient)
    {
        if (!CanUnlock(ingredient))
            return false;

        //임시 로드 추후에 수정해야함
        CarriageIngredientData cIData =
        DataManager.Instance.carriageUpgrades.Find(
        x => x != null &&
            x.ingredient != null &&
            x.ingredient.ingredientId == ingredient.ingredientId
        );
        if (cIData == null)
            return false;

        if (!curData.SpendGold(cIData.unlockGoldCost))
            return false;

        curData.UnlockIngredient(ingredient.ingredientId);

        OnIngredientUnlocked?.Invoke(ingredient);

        return true;
    }

    public bool IsUnlocked(IngredientData ingredient)
    {
        if (ingredient == null)
            return false;

        return curData.IsIngredientUnlocked(
            ingredient.ingredientId
        );
    }
}