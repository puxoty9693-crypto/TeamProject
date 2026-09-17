using UnityEngine;
using System.Collections.Generic;
using System;



public static class UpgradeHelper
{
    public static bool TryUpgrade<T>(int currentLevel, List<T> levels, Action onSuccess) where T : IUpgradeLevel 
    {
        if (currentLevel >= levels.Count) return false; //이미 최대 레벨

        int cost = levels[currentLevel].UpgradeGoldCost;
        if (!SaveManager.Instance.CurrentData.SpendGold(cost)) return false; //골드 부족

        onSuccess?.Invoke(); //실제 레벨업 실행
        return true;
    }
}
