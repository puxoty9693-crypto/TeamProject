using System;
using System.Collections.Generic;
using UnityEngine;

public class IngredientBox
{
    // 재료 상자에 현재 들어있는 재료 목록
    private readonly List<IngredientStock> ingredientStocks =
        new List<IngredientStock>();

    // 재료가 상자에 추가되었을 때
    public event Action<IngredientData, int> OnIngredientAdded;

    // NPC가 상자 안의 재료를 전부 가져갔을 때
    public event Action<IReadOnlyList<IngredientStock>> OnIngredientsTaken;


    // 재료를 상자에 추가
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

        IngredientStock stock =
            ingredientStocks.Find(
                s => s.ingredientId == ingredient.ingredientId
            );

        if (stock != null)
        {
            stock.count += amount;
        }
        else
        {
            ingredientStocks.Add(
                new IngredientStock
                {
                    ingredientId = ingredient.ingredientId,
                    count = amount
                }
            );
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

    // 특정 재료의 현재 수량 확인
    public int GetIngredientCount(IngredientData ingredient)
    {
        if (ingredient == null)
            return 0;

        IngredientStock stock =
            ingredientStocks.Find(
                s => s.ingredientId == ingredient.ingredientId
            );

        return stock != null ? stock.count : 0;
    }


    // 상자 안의 모든 재료를 한 번에 가져감
    public List<IngredientStock> TakeAllIngredients()
    {
        if (ingredientStocks.Count == 0)
            return null;

        // 현재 데이터를 복사
        List<IngredientStock> takenIngredients =
            new List<IngredientStock>();

        foreach (IngredientStock stock in ingredientStocks)
        {
            takenIngredients.Add(
                new IngredientStock
                {
                    ingredientId = stock.ingredientId,
                    count = stock.count
                }
            );
        }

        // 상자는 비움
        ingredientStocks.Clear();

        OnIngredientsTaken?.Invoke(
            takenIngredients
        );

        return takenIngredients;
    }
}