using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;

public class QuestManager
{
    private readonly PlayerData playerData;
    private readonly SuddenQuestConfig config;

    private readonly Queue<(float time, int totalGold)> incomeHistory = new Queue<(float, int)>(); // 최근 수입 추적용 (누적 골드, 기록 시점의 누적 플레이 시간)

    private float playTime;
    private float nextQuestTime;

    private bool isChallengeActive;
    public bool IsChallengeActive => isChallengeActive;

    private float challengeTimeRemaining;
    public float ChallengeTimeRemaining => challengeTimeRemaining;

    private int challengeStartGold;
    public int ChallengeGoalGold => challengeGoalGold;

    private int challengeGoalGold;
    public int ChallengeStartGold => challengeStartGold;

    // 돌발 퀘스트 시작 (목표 금액, 제한 시간)
    public event Action<int, float> OnQuestStarted;

    // 돌발 퀘스트 종료 (성공 여부 판단)
    public event Action<bool> OnQuestEnded;

    // 보상 지급
    public event Action<QuestRewardType, float> OnRewardGranted;

    public QuestManager(PlayerData data, SuddenQuestConfig config)
    {
        playerData = data;
        this.config = config;

        ScheduleNextQuest();
    }

    public void Update(float deltaTime)
    {
        playTime += deltaTime;

        RecordIncome();

        if (isChallengeActive)
        {
            UpdateChallenge(deltaTime);
        }
        else if (playTime >= nextQuestTime)
        {
            StartChallenge();
        }
    }

    private void ScheduleNextQuest()
    {
        float interval = UnityEngine.Random.Range(config.minIntervalSeconds, config.maxIntervalSeconds);

        nextQuestTime = playTime + interval;
    }
    
    private void RecordIncome() 
    {
        int currentGold = playerData.Gold;
        incomeHistory.Enqueue((playTime, currentGold));

        while (incomeHistory.Count > 0 && playTime - incomeHistory.Peek().time > config.incomeRefrenceWindowSeconds) 
        {
            incomeHistory.Dequeue();
        }
    }


    private void StartChallenge()
    {
        int currentGold = playerData.Gold;

        // 최근 구간 동안의 평균 초당 수입으로 목표 금액 계산
        float oldestTime = incomeHistory.Count > 0 ? incomeHistory.Peek().time : playTime;
        int oldestGold = incomeHistory.Count > 0 ? incomeHistory.Peek().totalGold : currentGold;

        float elapsed = Mathf.Max(playTime - oldestTime, 1f);
        float avgPerSecond = (currentGold - oldestGold) / elapsed;

        challengeGoalGold = Mathf.CeilToInt(avgPerSecond * config.challengeDurationSeconds * config.incomeGoalMultiplier);

        challengeGoalGold = Mathf.Max(challengeGoalGold, 1); // 최소 목표 1 골드 이상

        challengeStartGold = currentGold;
        challengeTimeRemaining = config.challengeDurationSeconds;
        isChallengeActive = true;

        OnQuestStarted?.Invoke(challengeGoalGold, config.challengeDurationSeconds);
    }

    private void UpdateChallenge(float deltaTime) 
    {
        challengeTimeRemaining -= deltaTime;

        if (challengeTimeRemaining > 0f)
            return;

        int earnGold = playerData.Gold - challengeStartGold;
        bool success = earnGold >= challengeGoalGold;

        isChallengeActive = false;
        OnQuestEnded?.Invoke(success);

        if (success) 
        {
            GrantRandomReward();
        }

        ScheduleNextQuest();
    }

    private void GrantRandomReward() 
    {
        var rewardTypes = (QuestRewardType[])Enum.GetValues(typeof(QuestRewardType));
        var rewardType = rewardTypes[Random.Range(0, rewardTypes.Length)];

        float value = rewardType switch
        {
            QuestRewardType.IncomeBonous => Random.Range(config.incomeBonusPercentMin, config.incomeBonusPercentMax + 1),

            QuestRewardType.IngredientSupplyBuff => config.ingredientSupplyMultiplier[Random.Range(0, config.ingredientSupplyMultiplier.Length)],

            QuestRewardType.FoodProductionBUff => config.foodProductionBonus[Random.Range(0, config.foodProductionBonus.Length)],
            _ => 0f

        };

        OnRewardGranted?.Invoke(rewardType,value);
    }

}


