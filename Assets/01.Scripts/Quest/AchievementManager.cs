using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager
{
    private readonly PlayerData playerData;
    private readonly AchievementConfig config;
    private readonly PaymentSystem paymentSystem;
    private readonly CraftingSystem craftingSystem;

    public event Action<AchievementTier, string> AchievementCompleted;

    public AchievementManager(PlayerData data, AchievementConfig config, PaymentSystem paymentSystem, CraftingSystem craftingSystem) 
    {
        playerData = data;
        this.config = config;
        this.paymentSystem = paymentSystem;
        this.craftingSystem = craftingSystem;

        paymentSystem.OnPaymentCompleted += HandlePaymentCompleted;

        craftingSystem.OnCookingCompleted += HandleCookingCompleted;

        CheckAllOnLoad();
    }

    //이벤트 핸들러

    private void HandlePaymentCompleted(FoodData food, int gold) 
    {
        playerData.Achievements.AddFoodSold(1);
        playerData.Achievements.AddIncome(gold); // 실제 판매 수입만 누적

        CheckCategory(config.foodSoldTiers, playerData.Achievements.TotalFoodSold, FoodSoldTitle);
        CheckCategory(config.totalIncomeTiers, playerData.Achievements.TotalIncome, TotalIncomeTitle);
    }

    private void HandleCookingCompleted(RecipeData recipe) 
    {
        playerData.Achievements.AddFoodCrafted(1);

        CheckCategory(config.foodCraftedTiers, playerData.Achievements.TotalFoodCrafted, FoodCraftedTitle);
    }

    //진행도 체크 / 보상 체크

    private void CheckAllOnLoad() 
    {
        CheckCategory(config.foodSoldTiers, playerData.Achievements.TotalFoodSold, FoodSoldTitle);
        CheckCategory(config.foodCraftedTiers, playerData.Achievements.TotalFoodCrafted, FoodCraftedTitle);
        CheckCategory(config.totalIncomeTiers, playerData.Achievements.TotalIncome, TotalIncomeTitle);
    }

    private void CheckCategory(List<AchievementTier> tiers, long currentValue, Func<long, string> titleFormatter) 
    {
        if (tiers == null) return;

        foreach (var tier in tiers) 
        {
            if (playerData.Achievements.IsCompledted(tier.id)) continue;
            if (currentValue < tier.threshold) continue;

            GrantTier(tier, titleFormatter(tier.threshold));
        }
    }

    private void GrantTier(AchievementTier tier, string title) 
    {
        playerData.Achievements.Complete(tier.id);

        if (tier.goldReward > 0)
            playerData.AddGold(tier.goldReward);

        AchievementCompleted?.Invoke(tier, title);
    }

    // 카테고리 별 UI 문구
    public static string FoodSoldTitle(long threshold) => $"음식 {threshold}개 판매하기";
    public static string FoodCraftedTitle(long threshold) => $"음식 {threshold}개 제작하기";
    public static string TotalIncomeTitle(long threshold) => $"수입 {threshold}골드 달성하기";


    public float PermanentBonus(QuestRewardType type) 
    {
        float bonus = 0f;
        bonus += CompletedSpecialBonus(config.foodSoldTiers, type);
        bonus += CompletedSpecialBonus(config.foodCraftedTiers, type);
        bonus += CompletedSpecialBonus(config.totalIncomeTiers, type);
        return bonus;
    }

    private float CompletedSpecialBonus(List<AchievementTier> tiers, QuestRewardType type) 
    {
        float sum = 0f;
        if (tiers == null) return sum;
        
        foreach (var tier in tiers) 
        {
            if (!tier.isSpecialTier) continue;
            if (tier.specialBuffType != type) continue;
            if (!playerData.Achievements.IsCompledted(tier.id)) continue;

            sum += tier.specialBuffBonus;
        }
        return sum;
    }
}
