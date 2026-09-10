using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TableUpgradeLevel : IUpgradeLevel
{
    public int tableCount;        // 이 레벨에서 설치 가능한 테이블 개수 (개별 테이블은 고정 4인)
    public int upgradeGoldCost;   // 다음 레벨 업그레이드 비용
    public int UpgradeGoldCost => upgradeGoldCost;
}

[CreateAssetMenu(fileName = "TableUpgrade", menuName = "Data/TableUpgrade")]
public class TableUpgradeData : ScriptableObject
{
    public List<TableUpgradeLevel> levels; // 레벨별 정보 (인덱스 = 레벨)
}