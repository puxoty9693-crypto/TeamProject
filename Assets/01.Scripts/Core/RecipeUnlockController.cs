using UnityEngine;

public class RecipeUnlockController : MonoBehaviour
{
    private RecipeUnlockSystem unlockSystem;

    public void Initialize(RecipeUnlockSystem system)
    {
        if (system == null)
        {
            Debug.LogError("RecipeUnlockSystem이 null입니다.");
            return;
        }

        unlockSystem = system;
    }

    public bool IsUnlocked(RecipeData recipe)
    {
        if (unlockSystem == null)
            return false;

        return unlockSystem.IsUnlocked(recipe);
    }

    public bool CanUnlock(RecipeData recipe)
    {
        if (unlockSystem == null)
            return false;

        return unlockSystem.CanUnlock(recipe);
    }

    public bool UnlockRecipe(RecipeData recipe)
    {
        if (unlockSystem == null)
            return false;

        return unlockSystem.UnlockRecipe(recipe);
    }
}