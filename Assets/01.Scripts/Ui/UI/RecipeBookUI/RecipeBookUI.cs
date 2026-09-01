using System.Collections.Generic;
using UnityEngine;

public class RecipeBookUI : MonoBehaviour
{
    [SerializeField] List<RecipeSlotUI> slots = new();

    void OnEnable()
    {
        SetRecipes();   
    }
    public void SetRecipes()
    {
        List<RecipeData> recipes = DataManager.Instance.allRecipes;

        for (int i = 0; i < recipes.Count && i < slots.Count; i++)
        {
            slots[i].UpdateRecipeUI(recipes[i]);
        }
    }
}
