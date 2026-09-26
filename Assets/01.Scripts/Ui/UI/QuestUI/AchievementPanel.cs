using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPanel : MonoBehaviour
{
    [SerializeField] AchievementRowUI rowPrefab;
    [SerializeField] Transform foodSoldContent;
    [SerializeField] Transform foodCraftedContent;
    [SerializeField] Transform totalIncomeContent;

    private readonly List<AchievementRowUI> spawnedRows = new List<AchievementRowUI>();

    private void OnEnable()
    {
        Refresh();

        if (GameManager.Instance != null && GameManager.Instance.AchievementManager != null)
            GameManager.Instance.AchievementManager.AchievementCompleted += HandleAchievementCompleted;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null && GameManager.Instance.AchievementManager != null)
            GameManager.Instance.AchievementManager.AchievementCompleted -= HandleAchievementCompleted;
        
    }

    private void HandleAchievementCompleted(AchievementTier tier, string title) 
    {
        Refresh();
    }

    public void Refresh() 
    {
        ClearRows();

        var config = DataManager.Instance.achievementConfig;
        var achievements = SaveManager.Instance.CurrentData.Achievements;

        BuildCategory(config.foodSoldTiers, achievements.TotalFoodSold, foodSoldContent, achievements, AchievementManager.FoodSoldTitle);
        BuildCategory(config.foodCraftedTiers, achievements.TotalFoodCrafted, foodCraftedContent, achievements, AchievementManager.FoodCraftedTitle);
        BuildCategory(config.totalIncomeTiers, achievements.TotalIncome, totalIncomeContent, achievements, AchievementManager.TotalIncomeTitle);
    }

    private void BuildCategory(List<AchievementTier> tiers, long currentValue, Transform parent, AchievementSaveData achievements, Func<long, string> titleFormatter) 
    {
        if (tiers == null || parent == null || rowPrefab == null) return;

        foreach (var tier in tiers) 
        {
            var row = Instantiate(rowPrefab, parent);
            bool completed = achievements.IsCompledted(tier.id);
            row.Setup(titleFormatter(tier.threshold), currentValue, tier.threshold, completed);
            spawnedRows.Add(row);
        }
    }

    private void ClearRows() 
    {
        foreach (var row in spawnedRows) 
        {
            if (row != null) Destroy(row.gameObject);
        }
        spawnedRows.Clear();
    }
}
