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

        return true;
    }

    public bool Unlock(IngredientData ingredient)
    {
        if (!CanUnlock(ingredient))
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