using UnityEngine;
using System;
using System.Collections.Generic;
public class AchievementTier : MonoBehaviour
{
    public string id;
    public long threshold; // 이 단계를 클리어하기 위한 목표치
    public int goldReward; // 달성 시 지급할 골드. 총 수입으론 포함 X
    public bool isSpecialTier;// 이 단계가 돌발 퀘스트 보상 버프를 영구적으로 강화시키는 단계인지
    public QuestRewardType specialBuffType; // 어느 버프 타입에 영향을 주는 지.
    public float specialBuffBonus; // 돌발 퀘스트에서 뽑힌 난수 값에서  영구적으로 더해지는 값.

}

[CreateAssetMenu(fileName = "AchievemetType", menuName = "Data/AchievementConfig")]
public class AchievementConfig : ScriptableObject
{
    [Header("음식 판매 갯수 업적")]
    public List<AchievementTier> foodSoldTiers = new List<AchievementTier>
    {
        new AchievementTier { id = "FoodSold_1", threshold = 10, goldReward = 1000},
        new AchievementTier { id = "FoodSold_2", threshold = 50, goldReward = 5000},
        new AchievementTier { id = "FoodSold_3", threshold = 100, goldReward = 10000},
        new AchievementTier { id = "FoodSold_4", threshold = 200, goldReward = 20000},
        new AchievementTier { id = "FoodSole_5", threshold = 500, goldReward = 0, isSpecialTier =true, specialBuffType =  QuestRewardType.FoodProductionBUff, specialBuffBonus = 2f},
    };

    [Header("음식 제작 갯수 업적")]
    public List<AchievementTier> foodCraftedTiers = new List<AchievementTier>
    {
        new AchievementTier {id = "FoodCrafted_1", threshold = 10, goldReward = 2500},
        new AchievementTier {id = "FoodCrafted_2", threshold = 50, goldReward = 7500},
        new AchievementTier {id = "FoodCrafted_3", threshold = 100, goldReward = 20000},
        new AchievementTier {id = "FoodCrafted_4", threshold = 200, goldReward = 50000},
        new AchievementTier {id = "FoodCrafted_5", threshold = 500, goldReward = 0, isSpecialTier =true, specialBuffType = QuestRewardType.IngredientSupplyBuff, specialBuffBonus =2f},
    };

    [Header("총 수입 업적")]
    public List<AchievementTier> totalIncomeTiers = new List<AchievementTier>
    {
        new AchievementTier {id = "TotalIncome_1", threshold =1000, goldReward = 500},
        new AchievementTier {id = "TotalIncome_2", threshold =10000, goldReward = 2000},
        new AchievementTier {id = "TotalIncome_3", threshold =100000, goldReward = 20000},
        new AchievementTier {id = "TotalIncome_4", threshold = 1000000, goldReward = 200000},
        new AchievementTier {id = "TotalIncome_5", threshold = 10000000, goldReward = 0, isSpecialTier = true, specialBuffType = QuestRewardType.IncomeBonus, specialBuffBonus = 2f},
    };
}
