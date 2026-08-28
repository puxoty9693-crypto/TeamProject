// 플레이어 전체 진행 상태 (JSON으로 저장/로드, SaveManager를 통해서만 접근 권장)
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int gold; // 보유 골드

    public List<string> unlockedRecipeIds;      // 해금한 레시피 ID 목록
    public List<string> unlockedIngredientIds;  // 해금한 재료 ID 목록
    public List<IngredientStock> warehouseStock; // 창고 재료별 수량

    public int carriageUpgradeLevel; // 마차 업그레이드 레벨
    public int adUpgradeLevel;       // 광고 업그레이드 레벨

    public List<NPCUpgradeSave> npcUpgradeLevels; // 역할별 NPC 업그레이드 레벨
}

