using System.Collections.Generic;
using UnityEngine;

// 특정 재료의 마차 수급 강화 한 단계
[System.Serializable]
public class CarriageIngredientLevel: IUpgradeLevel
{
    public float gatherTime;       // 이 레벨에서 재료 하나 생산까지 걸리는 시간 (예: 15초 → 10초)
    public int upgradeGoldCost;    // 다음 레벨로 올리는 데 필요한 골드
    public bool isInfinite;        // 최종 레벨 여부 (true면 무한 생산)
    public int UpgradeGoldCost => upgradeGoldCost;
}

// 재료 하나에 대한 마차 강화 전체 단계
[CreateAssetMenu(fileName = "CarriageUpgrade_", menuName = "Data/CarriageUpgrade")]
public class CarriageIngredientData : ScriptableObject
{
    public IngredientData ingredient;               // 어떤 재료에 대한 강화인지
    public int unlockGoldCost; //재료 해금하는 비용
    public List<CarriageIngredientLevel> levels;     // 레벨별 정보 (인덱스 = 레벨)
}