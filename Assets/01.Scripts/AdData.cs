// 광고 업그레이드 한 단계의 정보
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AdUpgradeLevel
{
    public int upgradeGoldCost;  // 다음 레벨 업그레이드 비용
    public string upgradeText;   // UI에 표시할 설명 텍스트
}

// 광고 전체 업그레이드 단계
[CreateAssetMenu(fileName = "Ad_", menuName = "Data/Ad")]
public class AdData : ScriptableObject
{
    public List<AdUpgradeLevel> levels; // 레벨별 정보 (인덱스 = 레벨)
}

