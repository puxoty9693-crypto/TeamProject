using UnityEngine;

public class RecipeManager : MMSingleton<RecipeManager>
{
    public bool IsUnlocked(string reipeId)
        => SaveManager.Instance.CurrentData.IsRecipeUnlocked(reipeId);

    // 이 레시피가 쓰는 재료가 전부 해금되어있는지 (AND 조건)
    public bool HasRequiredIngredientsUnlocked(RecipeData data)
    {
        foreach (var req in data.food.requiredIngredients)
        {
            if (!CarriageManager.Instance.IsUnlocked(req.ingredient.ingredientId))
                return false;
        }
        return true;
    }

    public bool UnlockRecipe(string recipeId) 
    {
        if (IsUnlocked(recipeId)) return false;

        var data = DataManager.Instance.allRecipes.Find(r => r.recipeId == recipeId);
        if (data == null) return false;

        if (!HasRequiredIngredientsUnlocked(data)) return false;

        if (!SaveManager.Instance.CurrentData.SpendGold(data.unlockGoldCost)) return false;

        SaveManager.Instance.CurrentData.UnlockedRecipe(recipeId);
        return true;
    }
      
    
}
