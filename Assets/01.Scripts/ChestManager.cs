// 창고(재료 적재) 관리. PlayerData.warehouseStock에 직접 접근하지 말고 항상 이 매니저를 거칠 것
using System.Collections.Generic;

public class ChestManager : MMSingleton<ChestManager>
{
    // 재료 추가 (파밍, 마차 수급 완료 시 호출)
    public void AddIngredient(string ingredientId, int amount)
    {
        var stock = GetStock(ingredientId);
        if (stock != null)
        {
            stock.count += amount;
        }
        else
        {
            SaveManager.Instance.CurrentData.warehouseStock.Add(
                new IngredientStock { ingredientId = ingredientId, count = amount }
            );
        }
    }

    // 재료 사용/소모 (음식 제작 시 호출), 재고 부족하면 false 반환
    public bool UseIngredient(string ingredientId, int amount)
    {
        var stock = GetStock(ingredientId);
        if (stock == null || stock.count < amount) return false;

        stock.count -= amount;
        return true;
    }

    // 현재 보유 수량 조회 (UI 표시용)
    public int GetIngredientCount(string ingredientId)
    {
        var stock = GetStock(ingredientId);
        return stock != null ? stock.count : 0;
    }

    // 내부용: ID로 재고 항목 찾기
    private IngredientStock GetStock(string ingredientId)
    {
        return SaveManager.Instance.CurrentData.warehouseStock
            .Find(s => s.ingredientId == ingredientId);
    }
}
