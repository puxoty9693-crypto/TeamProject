using UnityEngine;
using System;
using System.Collections.Generic;


public class AchievementSaveData
{
    // 업적 진행 카운터
    [SerializeField] private int totalFoodSold;
    public int TotalFoodSold => totalFoodSold;
    public void AddFoodSold(int amount)
    {
        if (amount > 0) totalFoodSold += amount;
    }

    [SerializeField] private int totalFoodCrafted;
    public int TotalFoodCrafted => totalFoodCrafted;
    public void AddFoodCrafted(int amount)
    {
        if (amount > 0) totalFoodCrafted += amount;
    }

    // 업적 퀘스트로 받은 골드는 여기 안 들어감
    [SerializeField] private long totalIncome;
    public long TotalIncome => totalIncome;
    public void AddIncome(long amount)
    {
        if (amount > 0) totalIncome += amount;
    }

    [SerializeField] private List<string> completeAchievementIds = new List<string>();
    public bool IsCompledted(string id) => completeAchievementIds.Contains(id);
    public void Complete(string id) 
    {
        if (!completeAchievementIds.Contains(id))
            completeAchievementIds.Add(id);
    }
}
