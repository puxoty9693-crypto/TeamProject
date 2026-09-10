using UnityEngine;

public class CraftingController : MonoBehaviour
{
    private CraftingSystem craftingSystem;
    public void Initialize(CraftingSystem system)
    {
        if (system == null)
        {
            Debug.LogError("CraftingSystem이 null입니다.");
            return;
        }

        craftingSystem = system;
    }

    private void Update()
    {
        if (craftingSystem == null)
            return;

        craftingSystem.Update(Time.deltaTime);
    }
    public void SelectRecipe(RecipeData recipe)
    {
        if (craftingSystem == null)
        {
            Debug.LogWarning("CraftingSystem이 초기화되지 않았습니다.");
            return;
        }

        craftingSystem.SelectRecipe(recipe);
    }

    public void ClearRecipe()
    {
        if (craftingSystem == null)
            return;

        craftingSystem.ClearRecipe();
    }

    public RecipeData SelectedRecipe =>
        craftingSystem?.SelectedRecipe;

    public RecipeData CookingRecipe =>
        craftingSystem?.CookingRecipe;

    public bool IsCooking =>
        craftingSystem != null && craftingSystem.IsCooking;

    public float CookingProgressRatio =>
        craftingSystem?.CookingProgressRatio ?? 0f;
}