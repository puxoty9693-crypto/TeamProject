using System;
using System.Collections.Generic;
using UnityEngine;

public class IngredientBox
{
    // 상자에 들어있는 재료 정보
    private readonly Dictionary<string, int> ingredientStocks =
        new Dictionary<string, int>();

    // 상자에 재료가 추가되었을 때
    public event Action<IngredientData, int> OnIngredientAdded;

    // NPC가 재료를 가져갔을 때
    public event Action<IngredientData, int> OnIngredientTaken;


    // 상자에 재료 추가
    public void AddIngredient(IngredientData ingredient, int amount)
    {
        if (ingredient == null)
        {
            Debug.LogWarning("추가하려는 재료가 null입니다.");
            return;
        }

        if (string.IsNullOrEmpty(ingredient.ingredientId))
        {
            Debug.LogWarning("재료 ID가 비어 있습니다.");
            return;
        }

        if (amount <= 0)
        {
            Debug.LogWarning("추가할 재료 수량은 1개 이상이어야 합니다.");
            return;
        }

        string ingredientId = ingredient.ingredientId;

        if (ingredientStocks.ContainsKey(ingredientId))
        {
            ingredientStocks[ingredientId] += amount;
        }
        else
        {
            ingredientStocks.Add(ingredientId, amount);
        }

        OnIngredientAdded?.Invoke(
            ingredient,
            amount
        );
    }


    // 해당 재료가 상자에 있는지 확인
    public bool HasIngredient(IngredientData ingredient)
    {
        if (ingredient == null)
            return false;

        if (string.IsNullOrEmpty(ingredient.ingredientId))
            return false;

        return GetIngredientCount(ingredient) > 0;
    }


    // 해당 재료의 현재 상자 수량 확인
    public int GetIngredientCount(IngredientData ingredient)
    {
        if (ingredient == null)
            return 0;

        if (string.IsNullOrEmpty(ingredient.ingredientId))
            return 0;

        if (ingredientStocks.TryGetValue(
                ingredient.ingredientId,
                out int count))
        {
            return count;
        }

        return 0;
    }


    // NPC가 재료 1개를 가져감
    public bool TakeIngredient(IngredientData ingredient)
    {
        if (!HasIngredient(ingredient))
            return false;

        string ingredientId = ingredient.ingredientId;

        ingredientStocks[ingredientId]--;

        // 0개가 되면 Dictionary에서 제거
        if (ingredientStocks[ingredientId] <= 0)
        {
            ingredientStocks.Remove(ingredientId);
        }

        OnIngredientTaken?.Invoke(
            ingredient,
            1
        );

        return true;
    }
}