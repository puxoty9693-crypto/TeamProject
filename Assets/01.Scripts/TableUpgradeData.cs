using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TableUpgradeLevel: IUpgradeLevel
{
    public int tableCount; //이 레벨에서 설치할 수 있는 테이블의 최대 개수
    public int upgradeGoldCost;
    public int UpgradeGoldCost => upgradeGoldCost;
}

[CreateAssetMenu(fileName = "TableUpgradeData", menuName = "Data/TableUpgrade"]
public class TableUpgradeData : MonoBehaviour
{
    public List<TableUpgradeLevel> levels;
}
