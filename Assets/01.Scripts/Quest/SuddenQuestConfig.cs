using UnityEngine;


// 돌발 퀘스트 완료 시 보상 받는 버프 종류
public enum QuestRewardType 
{
    IncomeBonus,                // 추가 수익 ( 1 ~ 100% 사이 난 수, 일정 시간 동안 음식 판매 수익에 +%로 더해짐)
    IngredientSupplyBuff,       // 재료 수급량 배수 버프 ( 1.5배 / 2배 / 3배 중 랜덤, 일정 시간 지속)
    FoodProductionBuff,         // 음식 제작량 버프 ( +1개 / +2개/ +3개 중 랜덤, 일정 시간 지속)
}

// 돌발 퀘스트 시스템 전체 설정값
[CreateAssetMenu(fileName = "SuddenQuestConfig", menuName = "Data/SuddenQuestConfig")]
public class SuddenQuestConfig : ScriptableObject
{
    public float minIntervalSeconds = 100f;  // 다음 돌발 퀘스트 등장까지 최소 시간

    public float maxIntrevalSeconds = 200f; // 다음 돌발 퀘스트 등장까지 최대 시간

    public float buffDurationSeconds = 60f; // 버프 지속 시간

    public float challengeDurationSeconds = 30f; // 돌발 퀘스트 제한 시간

    public float goldMultiplier = 0.8f; // 목표 금액 = 최근 60초간 평균 초당 수입의 80%

    public float referenceWindow = 60f; // 최근 수입을 판단하는 구간

    public int bonusPercentMin = 1;
    public int bonusPercentMax = 100;

    public float[] ingredientSupplyMultiplier = { 1.5f, 2f, 3f };

    public int[] foodProductionBonus = { 1, 2, 3 };
}
