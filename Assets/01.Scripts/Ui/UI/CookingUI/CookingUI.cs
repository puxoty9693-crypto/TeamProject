using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingUI : MonoBehaviour
{
    [SerializeField] List<CookingRecipeSlot> resipeSlot = new();
    private void OnEnable()
    {
        SetRecipes();
    }

    public void SetRecipes()
    {
        IReadOnlyList<string> unlockedIDs = SaveManager.Instance.CurrentData.UnlockedRecipeIds;
        List<RecipeData> allRecipes = DataManager.Instance.allRecipes;

        int index = 0;

        foreach(string id in unlockedIDs)
        {
            RecipeData recipe = allRecipes.Find(x => x.recipeId == id);

            if (recipe != null && index < resipeSlot.Count)
            {
                    resipeSlot[index].SetRecipe(recipe);
                    index++;
            }
        }
    }
}
