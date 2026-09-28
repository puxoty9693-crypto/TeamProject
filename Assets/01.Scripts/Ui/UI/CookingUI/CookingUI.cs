using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CookingUI : MonoBehaviour
{

    [Header("토글에 있는 레시피슬롯들 연결")]
    [SerializeField] TMP_Dropdown recipeDropdown;
    [SerializeField] CookingRecipeInfo cookingInfo;


    List<RecipeData> unlockResipe = new();

    private void OnEnable()
    {
        recipeDropdown.onValueChanged.AddListener(OnRecipeSelected);

        var system = GameManager.Instance.CraftingSystem;
        system.OnCookingCanceled += OnCookingEnded;
        system.OnCookingCompleted += OnCookingEnded;

        var controller = GameManager.Instance.craftingController;
        if (controller.IsCooking)
        {
            RefreshRecipeList();
            SelectInDropdown(controller.CookingRecipe);
            cookingInfo.RestoreCurrentCooking();
        }
        else
        {
            SetRecipes();
        }
    }
    private void OnDisable()
    {
        recipeDropdown.onValueChanged.RemoveListener(OnRecipeSelected);
        if (GameManager.HasInstance && GameManager.Instance.CraftingSystem != null)
        {
            var system = GameManager.Instance.CraftingSystem;
            system.OnCookingCanceled -= OnCookingEnded;
            system.OnCookingCompleted -= OnCookingEnded;
        }
    }
    public void SetRecipes()
    {
        IReadOnlyList<string> unlockedIDs = SaveManager.Instance.CurrentData.UnlockedRecipeIds;
        List<RecipeData> allRecipes = DataManager.Instance.allRecipes;

        unlockResipe.Clear();
        List<string> optionLabels = new();

        foreach (string id in unlockedIDs)
        {
            RefreshRecipeList();
            if (unlockResipe.Count > 0)
                cookingInfo.UpdateCookingInfo(unlockResipe[0]);
        }

    }
    private void RefreshRecipeList()
    {
        IReadOnlyList<string> unlockedIDs = SaveManager.Instance.CurrentData.UnlockedRecipeIds;
        List<RecipeData> allRecipes = DataManager.Instance.allRecipes;

        unlockResipe.Clear();
        List<string> optionLabels = new();

        foreach (string id in unlockedIDs)
        {
            RecipeData recipe = allRecipes.Find(x => x.recipeId == id);
            if (recipe == null)
                continue;
            unlockResipe.Add(recipe);
            optionLabels.Add(recipe.food.foodName);
        }
        recipeDropdown.ClearOptions();
        recipeDropdown.AddOptions(optionLabels);
    }
    private void SelectInDropdown(RecipeData recipe)
    {
        int idx = unlockResipe.IndexOf(recipe);
        if (idx >= 0)
            recipeDropdown.SetValueWithoutNotify(idx);
    }

    private void OnCookingEnded(RecipeData recipe)
    {
        RefreshRecipeList();
        SelectInDropdown(recipe);
    }
    private void OnRecipeSelected(int index)
    {
        if (index < 0 || index >= unlockResipe.Count)
            return;
        cookingInfo.UpdateCookingInfo(unlockResipe[index]);
    }
}