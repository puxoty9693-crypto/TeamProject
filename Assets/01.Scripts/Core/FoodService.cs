using System;
using UnityEngine;

public class FoodService
{
    private readonly PlayerData curData;

    public FoodService(PlayerData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        curData = data;
    }

    // 해당 음식이 창고에 있는지 확인
    public bool HasFood(FoodData food)
    {
        if (food == null)
            return false;

        if (string.IsNullOrEmpty(food.foodId))
            return false;

        return curData.GetFoodCount(food.foodId) > 0;
    }

    // 해당 음식의 현재 보유 수량 확인
    public int GetFoodCount(FoodData food)
    {
        if (food == null)
            return 0;

        if (string.IsNullOrEmpty(food.foodId))
            return 0;

        return curData.GetFoodCount(food.foodId);
    }

    // 음식 1개를 창고에서 가져감
    public bool TakeFood(FoodData food)
    {
        if (food == null)
            return false;

        if (string.IsNullOrEmpty(food.foodId))
            return false;

        return curData.UseFood(food.foodId, 1);
    }
}