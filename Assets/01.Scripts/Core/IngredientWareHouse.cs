using System.Collections.Generic;

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