using System;
using UnityEngine;

public class SellingSystem
{
    private readonly PlayerData curData;

    public event Action<FoodData, int> OnFoodSold;
    public event Action<int> OnGoldEarned;

    public SellingSystem(PlayerData data)
    {
        if (data == null)
        {
            Debug.LogError("PlayerData가 null입니다.");
            return;
        }

        curData = data;
    }
    public bool CanSell(FoodData food, int amount)
    {
        if (food == null)
            return false;

        if (amount <= 0)
            return false;

        return curData.GetFoodCount(food.foodId) >= amount;
    }

    public bool Sell(FoodData food, int amount = 1)
    {
        if (!CanSell(food, amount))
            return false;

        // 음식 차감
        if (!curData.UseFood(food.foodId, amount))
            return false;

        // 판매 금액 계산
        int totalGold = food.goldPerSale * amount;

        // 골드 지급
        curData.AddGold(totalGold);

        // 이벤트 전달
        OnFoodSold?.Invoke(food, amount);
        OnGoldEarned?.Invoke(totalGold);

        return true;
    }

    public int GetSellableAmount(FoodData food)
    {
        if (food == null)
            return 0;

        return curData.GetFoodCount(food.foodId);
    }
}