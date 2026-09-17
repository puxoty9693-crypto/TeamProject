using System;
using UnityEngine;

public class PaymentSystem
{
    private readonly PlayerData curData;

    //결제 했을때
    public event Action<FoodData, int> OnPaymentCompleted;
    //골드 획득
    public event Action<int> OnGoldEarned;

    public PaymentSystem(PlayerData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        curData = data;
    }

    public bool CanPay(FoodData food)
    {
        if (food == null)
            return false;

        return food.goldPerSale > 0;
    }

    public int GetPrice(FoodData food)
    {
        if (food == null)
            return 0;

        return food.goldPerSale;
    }

    public bool Pay(FoodData food)
    {
        if (!CanPay(food))
            return false;

        int gold = GetPrice(food);
        curData.AddGold(gold);

        EventManager.Instance.PostNotification(EventType.OnChangeGold, null, curData.Gold);
        EventManager.Instance.PostNotification(EventType.OnGetGold, null);

        OnPaymentCompleted?.Invoke(food, gold);
        OnGoldEarned?.Invoke(gold);

        return true;
    }
}