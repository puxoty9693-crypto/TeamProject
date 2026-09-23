using UnityEngine;


//돌발 퀘스트 클리어시 부여 받는 버프
public enum QuestRewardType
{
    IncomeBonous,           // 추가 수익 (1 ~ 100 사이 난수, 일정 시간 동안 음식 판매 수익에 +% 더해짐)
    IngredientSupplyBuff,   // 재료 수급량 배수 버프 (1.5 배/ 2배 / 3배 중 랜덤, 일정 시간 동안 지속)
    FoodProductionBUff,     // 음식 제작량 버프
}

public enum QuestType
{
    EarnGoldWithTime,    // 제한 시간 안에 목표 금액 벌기
    SellFoodCount,       // 제한 시간 안에 목표 개수만큼 음식 판매
}

// 돌발 퀘스트 시스템 전체 설정값

[CreateAssetMenu(fileName = "SuddenQuestConfig", menuName = "Data/SuddenQuestConfig")]
public class SuddenQuestConfig : ScriptableObject
{
    public float minIntervalSeconds = 300f; // 다음 돌발 퀘스트 최소 등장 시간

    public float maxIntervalSeconds = 400f; // 다음 돌발 퀘스트 최종 등장 시간

    public int foodSellCountMin = 1;    //음식 판매 퀘스트 개수 최소
    public int foodSellCountMax = 30;   //음식 판매 퀘스트 개수 최대


    public float buffDurationSeconds = 60f; // 초 단위, 세 보상 타입 전부 이 시간만큼 적용됨

    public float challengeDurationSeconds = 30f; // 챌린지 제한시간

    public float incomeGoalMultiplier = 0.8f; // 목표 금액

    public float incomeRefrenceWindowSeconds = 60f; // 최근 수입

    public float foodSellGoalMultiplier = 0.8f; // 목표 음식 판매량

    public int incomeBonusPercentMin = 1;  //추가 수익 버프 - 난수 최소 범위

    public int incomeBonusPercentMax = 100; //추가 수익 버프 난수 최대 범위

    public float[] ingredientSupplyMultiplier = { 1.5f, 2f, 3f }; // 재료 수급량 버프 증가량

    public int[] foodProductionBonus = { 1, 2, 3 }; // 음식 제작량 버프 증가량




}
