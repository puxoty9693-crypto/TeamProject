using System;
using UnityEngine;

public class Inventory
{
    // 재료 수량이 변경되었을 때 실행되는 함수, UI 사용
    public event Action<IngredientData, int> OnIngredientCountChanged;

    private readonly PlayerData curData;

    public Inventory(PlayerData data)
    {
        curData = data;
    }

    public void AddIngredient(IngredientData ingredient, int amount)
    {
        if (amount <= 0 || ingredient == null)
        {
            Debug.LogWarning("재료가 존재하지 않거나 추가할 재료의 수량은 1 미만 입니다.");
            return;
        }
        string ingredientId = ingredient.ingredientId;

        curData.AddIngredient(ingredientId, amount);

        OnIngredientCountChanged?.Invoke(
            ingredient,
            curData.GetIngredientCount(ingredientId)
        );
    }


    public bool HasIngredient(IngredientData ingredient, int amount)
    {
        if (amount <= 0 || ingredient == null)
            return false;
        
        return curData.GetIngredientCount(ingredient.ingredientId) >= amount;
    }


    public bool RemoveIngredient(IngredientData ingredient, int amount)
    {
        if (amount <= 0 || ingredient == null)
            return false;

        curData.UseIngredient(ingredient.ingredientId, amount);

        OnIngredientCountChanged?.Invoke(
            ingredient,
            curData.GetIngredientCount(ingredient.ingredientId)
        );

        return true;
    }
}