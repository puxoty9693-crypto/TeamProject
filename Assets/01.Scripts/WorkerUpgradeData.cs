using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorkerUpgradeLevel : IUpgradeLevel
{
    public int upgradeValue;     // 적용되는 능력치(속도, 효율 등)
    public int upgradeGoldCost;  // 다음 레벨 업그레이드 비용
    public int UpgradeGoldCost => upgradeGoldCost;

}

// 특정 역할(WorkerRole)의 전체 업그레이드 단계
[CreateAssetMenu(fileName = "WorkerUpgrade_", menuName = "Data/WorkerUpgrade")]
public class WorkerUpgradeData : ScriptableObject
{
    public WorkerRole role;                    // 업그레이드 대상 역할
    public List<WorkerUpgradeLevel> levels;    // 레벨별 정보 (인덱스 = 레벨)
}