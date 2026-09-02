using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TableLevel : IUpgradeLevel
{
    public int capacity;          // 이 레벨에서 수용 가능한 손님 수
    public int upgradeGoldCost;   // 다음 레벨(수리/강화) 비용
    public Sprite tableSprite;    // 부서진 테이블 / 수리된 테이블 등 상태별 이미지
    public int UpgradeGoldCost => upgradeGoldCost;
}



[CreateAssetMenu(fileName = "Table_", menuName = "Data/Table")]
public class TableData : ScriptableObject
{
    public string tableId;             // 테이블 고유 ID (여러 개 배치되므로 구분 필요)
    public List<TableLevel> levels;    // 0레벨 = 부서진 상태, 1레벨부터 수리 완료
}
