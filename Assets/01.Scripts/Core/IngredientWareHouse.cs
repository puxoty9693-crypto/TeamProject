using System.Collections.Generic;
using System.Diagnostics;

public class IngredientWareHouse
{
    private readonly PlayerData curData;

    public IngredientWareHouse(PlayerData data)
    {
        curData = data;
    }

    public void ReceiveBox(IngredientBox box)
    {
        if (box == null)
            return;

        IReadOnlyList<IngredientStock> stocks =
            box.TakeAllIngredients();

        if (stocks == null || stocks.Count == 0) return;

        foreach (IngredientStock stock in stocks)
        {
            if (stock == null)
                continue;

            if (string.IsNullOrEmpty(stock.ingredientId))
                continue;

            if (stock.count <= 0)
                continue;

            curData.AddIngredient(
                stock.ingredientId,
                stock.count
            );
        }
        EventManager.Instance.PostNotification(EventType.OnWarehouseChanged, null);
    }
}