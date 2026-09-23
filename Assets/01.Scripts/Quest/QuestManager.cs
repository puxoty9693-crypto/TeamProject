using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;
using NUnit.Framework;
using Unity.Jobs.LowLevel.Unsafe;

public class QuestManager
{
    private readonly PlayerData playerData;
    private readonly SuddenQuestConfig config;
    private readonly PaymentSystem paymentSystem;

    private readonly Queue<(float time, int totalGold, int totalSold)> incomeHistory = new Queue<(float, int, int)>(); // 최근 수입 추적용 (누적 골드, 기록 시점의 누적 플레이 시간)

    private float playTime;
    private float nextQuestTime;
    private int soldCount; // 지금까지 팔린 음식 총 개수
    

    private bool isChallengeActive;
    public bool IsChallengeActive => isChallengeActive;

    private float challengeTimeRemaining;
    public float ChallengeTimeRemaining => challengeTimeRemaining;

    
    private int challengeStartGold;
    private int challengeSoldCount;
    public int ChallengeGoalValue => challengeGoalValue;

    public int ChallengeAchieved => currentType == QuestType.EarnGoldWithTime ? playerData.Gold - challengeStartGold : soldCount - challengeSoldCount;

    private QuestType currentType;
    public QuestType CurrentType => currentType;

    private int challengeGoalValue;
    public int ChallengeStartGold => challengeStartGold;




    //방금 끝난 돌발 퀘스트의 성공 판단
    public bool IsQuestCompleted {  get; private set; }

    // 돌발 퀘스트 시작 (목표 금액, 제한 시간)
    public event Action<int, float> OnQuestStarted;

    // 돌발 퀘스트 종료 (성공 여부 판단)
    public event Action<bool> OnQuestEnded;

    // 보상 지급
    public event Action<QuestRewardType, float> OnRewardGranted;

    public QuestManager(PlayerData data, SuddenQuestConfig config, PaymentSystem paymentSystem)
    {
        playerData = data;
        this.config = config;
        this.paymentSystem = paymentSystem;

        paymentSystem.OnPaymentCompleted += PaymentCompleted;
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

    private void PaymentCompleted(FoodData food, int gold) 
    {
        soldCount++;
    }


    private void ScheduleNextQuest()
    {
        float interval = UnityEngine.Random.Range(config.minIntervalSeconds, config.maxIntervalSeconds);

        nextQuestTime = playTime + interval;
    }
    
    private void RecordIncome() 
    {
        int currentGold = playerData.Gold;
        incomeHistory.Enqueue((playTime, currentGold, soldCount));

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

        var conditionTypes = (QuestType[])Enum.GetValues(typeof(QuestType));
        currentType = conditionTypes[Random.Range(0, conditionTypes.Length)];

        if (currentType == QuestType.EarnGoldWithTime) 
        {
            challengeGoalValue = Mathf.CeilToInt(avgPerSecond * config.challengeDurationSeconds * config.incomeGoalMultiplier);
        }
        else 
        {
            challengeGoalValue = Random.Range( config.foodSellCountMin, config.foodSellCountMax + 1);
        }

        challengeGoalValue = Mathf.Max(challengeGoalValue, 1); // 최소 목표 1 이상

        challengeStartGold = currentGold;
        challengeSoldCount = soldCount;
        challengeTimeRemaining = config.challengeDurationSeconds;
        isChallengeActive = true;
        IsQuestCompleted = false; // 새 퀘스트 시작하니까 초기화
        GameLogOnlyEditor.Log($"[Quest] 뽑힌 조건: {currentType} / 목표값: {challengeGoalValue}");
        OnQuestStarted?.Invoke(challengeGoalValue, config.challengeDurationSeconds);
    }

    private void UpdateChallenge(float deltaTime) 
    {
        challengeTimeRemaining -= deltaTime;

        int achievedValue = ChallengeAchieved;
        bool goalReached = achievedValue >= challengeGoalValue;

        if (goalReached) 
        {
            FinishChallenge(true, achievedValue);
            return;
        }

        //목표를 아직 못 채웠으면, 시간이 남아있는 동안 대기
        if (challengeTimeRemaining > 0f)
            return;

        //시간이 다 됐는데도 목표 미달성
        FinishChallenge(false, achievedValue);

    }

    // 돌발 퀘스트 성공/실패 공통처리
    private void FinishChallenge(bool success, int earnGold) 
    {
        isChallengeActive = false;
        IsQuestCompleted = success;

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


