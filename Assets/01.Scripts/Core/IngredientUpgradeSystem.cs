using System;
using UnityEngine;

public class IngredientUpgradeSystem
{
    private readonly PlayerData curData;

    public event Action<IngredientData, int> OnIngredientUpgraded;

    public IngredientUpgradeSystem(PlayerData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        curData = data;
    }

    // 해당 재료의 마차 업그레이드 가능 여부
    public bool CanUpgrade(IngredientData ingredient)
    {
        if (ingredient == null)
            return false;

        if (string.IsNullOrEmpty(ingredient.ingredientId))
            return false;

        // 해금되지 않은 재료는 업그레이드 불가
        if (!curData.IsIngredientUnlocked(ingredient.ingredientId))
            return false;

        CarriageIngredientData cIData =
            GetCarriageIngredientData(ingredient);

        if (cIData == null)
            return false;

        int currentLevel =
            curData.GetCarriageLevel(ingredient.ingredientId);

        // 마지막 레벨이면 업그레이드 불가
        if (IsMaxLevel(cIData, currentLevel))
            return false;

        int upgradeCost =
            GetUpgradeCost(cIData, currentLevel);

        if (upgradeCost < 0)
            return false;

        // 골드 부족
        if (curData.Gold < upgradeCost)
            return false;

        return true;
    }

    // 실제 업그레이드 실행
    public bool Upgrade(IngredientData ingredient)
    {
        if (!CanUpgrade(ingredient))
            return false;

        CarriageIngredientData cIData =
            GetCarriageIngredientData(ingredient);

        if (cIData == null)
            return false;

        int currentLevel =
            curData.GetCarriageLevel(ingredient.ingredientId);

        int upgradeCost =
            GetUpgradeCost(cIData, currentLevel);

        if (upgradeCost < 0)
            return false;

        // 골드 차감 실패 시 업그레이드 중단
        if (!curData.SpendGold(upgradeCost))
            return false;

        // 현재 레벨을 다음 레벨로 증가
        curData.UpgradeCarriageLevel(ingredient.ingredientId);

        int nextLevel =
            curData.GetCarriageLevel(ingredient.ingredientId);

        OnIngredientUpgraded?.Invoke(
            ingredient,
            nextLevel
        );

        return true;
    }

    // 현재 레벨 조회
    public int GetCurrentLevel(IngredientData ingredient)
    {
        if (ingredient == null)
            return 0;

        if (string.IsNullOrEmpty(ingredient.ingredientId))
            return 0;

        return curData.GetCarriageLevel(ingredient.ingredientId);
    }

    // 현재 레벨에서 적용되는 수급량 조회
    public int GetCurrentGatherAmount(IngredientData ingredient)
    {
        CarriageIngredientData cIData =
            GetCarriageIngredientData(ingredient);

        if (cIData == null)
            return 0;

        int currentLevel =
            GetCurrentLevel(ingredient);

        return cIData.GetGatherAmount(currentLevel);
    }

    // 다음 업그레이드 비용 조회
    public int GetNextUpgradeCost(IngredientData ingredient)
    {
        CarriageIngredientData cIData =
            GetCarriageIngredientData(ingredient);

        if (cIData == null)
            return -1;

        int currentLevel =
            GetCurrentLevel(ingredient);

        if (IsMaxLevel(cIData, currentLevel))
            return -1;

        return GetUpgradeCost(cIData, currentLevel);
    }

    // 최대 레벨 여부
    public bool IsMaxLevel(IngredientData ingredient)
    {
        CarriageIngredientData cIData =
            GetCarriageIngredientData(ingredient);

        if (cIData == null)
            return false;

        int currentLevel =
            GetCurrentLevel(ingredient);

        return IsMaxLevel(cIData, currentLevel);
    }

    private int GetUpgradeCost(
        CarriageIngredientData cIData,
        int currentLevel)
    {
        if (cIData.levels == null)
            return -1;

        if (currentLevel < 0 ||
            currentLevel >= cIData.levels.Count)
        {
            return -1;
        }

        return cIData.levels[currentLevel].upgradeGoldCost;
    }

    private bool IsMaxLevel(
        CarriageIngredientData cIData,
        int currentLevel)
    {
        if (cIData.levels == null ||
            cIData.levels.Count == 0)
        {
            return true;
        }

        return currentLevel >= cIData.levels.Count - 1;
    }

    private CarriageIngredientData GetCarriageIngredientData(
        IngredientData ingredient)
    {
        if (ingredient == null)
            return null;

        if (DataManager.Instance == null)
            return null;

        if (DataManager.Instance.carriageUpgrades == null)
            return null;

        return DataManager.Instance.carriageUpgrades.Find(
            x => x != null &&
                 x.ingredient != null &&
                 x.ingredient.ingredientId == ingredient.ingredientId
        );
    }
}