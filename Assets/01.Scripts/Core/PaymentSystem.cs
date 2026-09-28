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

        return food.sellPrice > 0;
    }

    public int GetPrice(FoodData food)
    {
        if (food == null)
            return 0;

        return food.sellPrice;
    }

    public bool Pay(FoodData food, Vector3 pos)
    {
        int gold = GetPrice(food);

        int upgradeGold = GameManager.Instance.workerUpgradeSystem.GetUpgradedIncome(gold);
        if(upgradeGold < 0) return false;

        curData.AddGold(upgradeGold);

        EventManager.Instance.PostNotification(EventType.OnChangeGold, null, curData.Gold);
        EventManager.Instance.PostNotification(EventType.OnGetGold, null, pos);

        OnPaymentCompleted?.Invoke(food, gold);
        OnGoldEarned?.Invoke(gold);

        return true;
    }
}