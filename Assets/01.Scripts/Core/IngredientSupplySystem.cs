using System;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSupplySystem
{
    private readonly PlayerData curData;
    private readonly List<IngredientData> ingredients;
    private readonly IngredientBox ingredientBox;

    // 기본 공급 주기
    private const float SupplyInterval = 5f;

    // 기본 공급량
    private const int BaseSupplyAmount = 1;

    // 재료별 공급 타이머
    private readonly Dictionary<string, float> supplyTimers
        = new Dictionary<string, float>();

    // 재료가 상자에 공급되었을 때
    public event Action<IngredientData, int> OnIngredientSupplied;

    public IngredientSupplySystem(
        PlayerData data,
        List<IngredientData> ingredients,
        IngredientBox ingredientBox)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        if (ingredients == null)
            throw new ArgumentNullException(nameof(ingredients));

        if (ingredientBox == null)
            throw new ArgumentNullException(nameof(ingredientBox));

        curData = data;
        this.ingredients = ingredients;
        this.ingredientBox = ingredientBox;

        // 모든 재료의 타이머 초기화
        foreach (var ingredient in ingredients)
        {
            if (ingredient == null)
                continue;

            if (string.IsNullOrEmpty(ingredient.ingredientId))
                continue;

            if (!supplyTimers.ContainsKey(ingredient.ingredientId))
                supplyTimers.Add(ingredient.ingredientId, 0f);
        }
    }

    public void Update(float deltaTime)
    {
        if (deltaTime <= 0f)
            return;

        foreach (var ingredient in ingredients)
        {
            if (ingredient == null)
                continue;

            string ingredientId = ingredient.ingredientId;

            if (string.IsNullOrEmpty(ingredientId))
                continue;

            // 해금된 재료만 공급
            if (!curData.IsIngredientUnlocked(ingredientId))
                continue;

            supplyTimers[ingredientId] += deltaTime;

            if (supplyTimers[ingredientId] < SupplyInterval)
                continue;

            // 5초 단위 유지
            supplyTimers[ingredientId] -= SupplyInterval;

            SupplyIngredient(ingredient);
        }
    }

    private void SupplyIngredient(IngredientData ingredient)
    {
        // 재료 업그레이드 레벨
        int upgradeLevel =
            curData.GetCarriageLevel(ingredient.ingredientId);

        // Lv.0 = 1개
        // Lv.1 = 2개
        // Lv.2 = 3개
        int supplyAmount =
            BaseSupplyAmount + upgradeLevel;

        ingredientBox.AddIngredient(
            ingredient,
            supplyAmount
        );

        OnIngredientSupplied?.Invoke(
            ingredient,
            supplyAmount
        );
    }

    // 현재 재료의 업그레이드 적용 공급량 확인
    public int GetSupplyAmount(IngredientData ingredient)
    {
        if (ingredient == null)
            return 0;

        int upgradeLevel =
            curData.GetCarriageLevel(ingredient.ingredientId);

        return BaseSupplyAmount + upgradeLevel;
    }

    // 현재 재료의 공급 주기
    public float GetSupplyInterval(IngredientData ingredient)
    {
        if (ingredient == null)
            return 0f;

        return SupplyInterval;
    }
}