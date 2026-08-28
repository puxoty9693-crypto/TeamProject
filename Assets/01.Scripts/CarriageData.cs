// 마차 업그레이드 한 단계의 정보
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarriageLevel
{
    public float gatherTime;      // 재료 수급 소요 시간
    public int ingredientAmount;  // 한 번에 수급되는 재료 수량
    public int upgradeGoldCost;   // 다음 레벨 업그레이드 비용
}

// 마차(재료 수급처) 전체 업그레이드 단계
[CreateAssetMenu(fileName = "Carriage_", menuName = "Data/Carriage")]
public class CarriageData : ScriptableObject
{
    public List<CarriageLevel> levels; // 레벨별 정보 (인덱스 = 레벨)
}
