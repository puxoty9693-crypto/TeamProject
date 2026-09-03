using System.Collections.Generic;
using UnityEngine;

public class RecipeBookUI : MonoBehaviour
{
    [SerializeField] List<RectTransform> pages = new();
    private const int slotsPerPage = 2;

    void OnEnable()
    {
        SetRecipes();   
    }
    public void SetRecipes()
    {
        List<RecipeData> recipes = DataManager.Instance.allRecipes;

        for (int i = 0; i < pages.Count; i++)
        {
            RecipeSlotUI[] slots = pages[i].GetComponentsInChildren<RecipeSlotUI>(true);
            for(int j = 0; j < slots.Length; j++)
            {
                int recipeIndex = i * slotsPerPage + j;

                if(recipeIndex < recipes.Count)
                {
                    slots[j].gameObject.SetActive(true);
                    RefreshSlot(recipes[recipeIndex], slots[j]);
                }
                else
                {
                    slots[j].gameObject.SetActive(false);
                }
            }
        }
    }
    private void RefreshSlot(RecipeData data, RecipeSlotUI slot)
    {
        bool isUnlocked = RecipeManager.Instance.IsUnlocked(data.recipeId);
        slot.UpdateRecipeUI(data, isUnlocked, () => TryUnlock(data, slot));
    }

    private void TryUnlock(RecipeData data, RecipeSlotUI slot)
    {
        bool success = RecipeManager.Instance.UnlockRecipe(data.recipeId);

        if (!success)
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "골드가 부족합니다");
            return;
        }

        EventManager.Instance.PostNotification(EventType.OnChangeGold, this, SaveManager.Instance.CurrentData.Gold);
        RefreshSlot(data, slot);
    }
}
