using System;
using UnityEngine;

public class RecipeUnlockSystem
{
    private readonly PlayerData curData;

    public event Action<RecipeData> OnRecipeUnlocked;

    public RecipeUnlockSystem(PlayerData data)
    {
        if (data == null)
        {
            Debug.LogError("PlayerData가 null입니다.");
            return;
        }

        curData = data;
    }

    public bool IsUnlocked(RecipeData recipe)
    {
        if (recipe == null)
            return false;

        return curData.IsRecipeUnlocked(recipe.recipeId);
    }

    public bool CanUnlock(RecipeData recipe)
    {
        if (recipe == null)
            return false;

        // 이미 해금된 레시피
        if (IsUnlocked(recipe))
            return false;

        // 골드 부족
        if (curData.Gold < recipe.unlockGoldCost)
            return false;

        return true;
    }

    public bool UnlockRecipe(RecipeData recipe)
    {
        if (!CanUnlock(recipe))
            return false;

        // 골드 차감
        if (!curData.SpendGold(recipe.unlockGoldCost))
            return false;

        // 레시피 해금
        curData.UnlockedRecipe(recipe.recipeId);

        OnRecipeUnlocked?.Invoke(recipe);

        return true;
    }
}