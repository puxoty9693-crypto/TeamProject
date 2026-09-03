using UnityEngine;

// [로직 - 저장 대상 아님] 음식 창고 관리 (ChestManager와 동일 패턴, 대상만 음식)
public class FoodStockManager : MMSingleton<FoodStockManager>
{
    public void AddFood(string foodId, int amount)
        => SaveManager.Instance.CurrentData.AddFood(foodId, amount);

    public bool UseFood(string foodId, int amount)
        => SaveManager.Instance.CurrentData.UseFood(foodId, amount);

    public int GetFoodCount(string foodId)
        => SaveManager.Instance.CurrentData.GetFoodCount(foodId);
}