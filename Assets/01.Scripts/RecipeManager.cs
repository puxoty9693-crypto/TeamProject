using UnityEngine;

public class RecipeManager : MMSingleton<RecipeManager>
{
    public bool IsUnlocked(string reipeId)
        => SaveManager.Instance.CurrentData.IsRecipeUnlocked(reipeId);

    public bool UnlockRecipe(string recipeId) 
    {
        if (IsUnlocked(recipeId)) return false;

        var data = DataManager.Instance.allRecipes.Find(r => r.recipeId == recipeId);
        if (data == null) return false;

        if (!SaveManager.Instance.CurrentData.SpendGold(data.unlockGoldCost)) return false;

        SaveManager.Instance.CurrentData.UnlockedRecipe(recipeId);
        return true;
    }
      
    
}
