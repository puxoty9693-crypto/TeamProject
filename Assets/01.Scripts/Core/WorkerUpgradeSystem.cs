using System;
using System.Data;
using UnityEngine;

public class WorkerUpgradeSystem
{
    private readonly PlayerData curData;

    public WorkerUpgradeSystem(PlayerData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        curData = data;
    }

    // 현재 레벨
    public int GetCurrentLevel(WorkerRole role)
    {
        return curData.GetWorkerUpgradeLevel(role);
    }

    // 현재 레벨의 업그레이드 값
    public float GetUpgradeValue(WorkerRole role)
    {
        WorkerUpgradeData data = GetUpgradeData(role);

        if (data == null)
            return 1f;

        int level = GetCurrentLevel(role);

        if (!IsValidLevel(data, level))
            return 1f;

        return data.levels[level].upgradeValue;
    }

    // 다음 업그레이드 비용
    public int GetNextUpgradeCost(WorkerRole role)
    {
        WorkerUpgradeData data = GetUpgradeData(role);

        if (data == null)
            return -1;

        int currentLevel = GetCurrentLevel(role);
        int nextLevel = currentLevel + 1;

        if (!IsValidLevel(data, currentLevel))
            return -1;

        return data.levels[currentLevel].upgradeGoldCost;
    }

    //현재 강화 배율값 리턴
    public float GetCurrentUpgradeValue(WorkerRole role)
    {
        WorkerUpgradeData data = GetUpgradeData(role);
        int currentLevel = GetCurrentLevel(role);

        float currentValue = data.levels[currentLevel].upgradeValue;

        if (currentValue < 0f) return 0f;
        return currentValue;
    }

    // 업그레이드 가능 여부
    public bool CanUpgrade(WorkerRole role)
    {
        WorkerUpgradeData data = GetUpgradeData(role);

        if (data == null)
            return false;

        int currentLevel = GetCurrentLevel(role);

        if (!IsValidLevel(data, currentLevel))
            return false;

        int cost = data.levels[currentLevel].upgradeGoldCost;

        return curData.Gold >= cost;
    }

    // 업그레이드 실행
    public bool Upgrade(WorkerRole role)
    {
        if (!CanUpgrade(role))
            return false;

        WorkerUpgradeData data = GetUpgradeData(role);

        int currentLevel = GetCurrentLevel(role);
        int nextLevel = currentLevel + 1;

        int cost = data.levels[currentLevel].upgradeGoldCost;

        if (!curData.SpendGold(cost))
            return false;

        curData.UpgradeWorkerLevel(role);

        return true;
    }

    // 해당 역할의 WorkerUpgradeData 찾기
    private WorkerUpgradeData GetUpgradeData(WorkerRole role)
    {
        if (DataManager.Instance == null)
            return null;

        if (DataManager.Instance.workerUpgradeDataList == null)
            return null;

        return DataManager.Instance.workerUpgradeDataList.Find(
            data => data != null &&
                    data.role == role
        );
    }

    private bool IsValidLevel(WorkerUpgradeData data,int level)
    {
        if (data.levels == null)
            return false;

        return level >= 0 &&
               level < data.levels.Count;
    }

    //강화 기준 요리 소모 시간 
    public float GetUpgradedRecipeTime(float originTime)
    {
        WorkerUpgradeData data = GetUpgradeData(WorkerRole.Chef);
        int currentLevel = GetCurrentLevel(WorkerRole.Chef);
        float value = data.levels[currentLevel].upgradeValue;

        float finalTime = originTime * (1f - data.levels[currentLevel].upgradeValue);

        if(finalTime < 0) return 0;

        return finalTime;
    }

    //가격 강화 기준 레시피 판매 가격
    public int GetUpgradedIncome(int originIncome)
    {
        if (originIncome <= 0) return 0;

        WorkerUpgradeData data = GetUpgradeData(WorkerRole.Cashier);
        int currentLevel = GetCurrentLevel(WorkerRole.Cashier);
        float value = data.levels[currentLevel].upgradeValue;

        if (value <= 0f) return originIncome;

        return Mathf.RoundToInt(originIncome * (1f + value));
    }

    //강화된 서빙 NPC 속도 값
    public float GetUpgradedServerSpeed(float originSpeed)
    {
        if (originSpeed <= 0f) return 0f;

        WorkerUpgradeData data =GetUpgradeData(WorkerRole.Server);

        int currentLevel =GetCurrentLevel(WorkerRole.Server);
        float value = data.levels[currentLevel].upgradeValue;

        if (value <= 0f) return originSpeed;

        return originSpeed * (1f + value);
    }
}