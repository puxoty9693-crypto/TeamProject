// 창고(재료 적재) 관리. PlayerData.warehouseStock에 직접 접근하지 말고 항상 이 매니저를 거칠 것
using System.Collections.Generic;

public class ChestManager : MMSingleton<ChestManager>
{
   public void AddIngredient(string ingredientId, int amount)
        => SaveManager.Instance.CurrentData.AddIngredient(ingredientId, amount);
   public bool UseIngredient(string ingredientId, int amount)
        => SaveManager.Instance.CurrentData.UseIngredient(ingredientId, amount);
   public int GetIngredientCount(string ingredientId)
        => SaveManager.Instance.CurrentData.GetIngredientCount(ingredientId);
}
