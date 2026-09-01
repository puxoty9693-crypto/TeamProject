// 플레이어 전체 진행 상태 (JSON으로 저장/로드, SaveManager를 통해서만 접근 권장)
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class IngredientStock
{
    public string ingredientId; // 재료 ID (IngredientData.ingredientId와 매칭)
    public int count;           // 현재 보유 수량
}

// 특정 NPC 역할의 현재 업그레이드 레벨
[System.Serializable]
public class NPCUpgradeSave
{
    public NPCRole role; // 업그레이드 대상 역할
    public int level;    // 현재 레벨
}

// 재료별 현재 마차 강화 레벨
[System.Serializable]
public class CarriageSave
{
    public string ingredientId;
    public int level;
}

// 개별 테이블의 현재 상태 저장용
[System.Serializable]
public class TableSave
{
    public string tableId;
    public int level; // 0 = 부서진 채로 방치, 1 이상 = 수리/강화된 레벨
}

// 창고로 옮기기 전, 마차 박스에 임시로 쌓인 재료
[System.Serializable]
public class CarriageBoxStock
{
    public string ingredientId;
    public int count;
}


public class PlayerData
{
    public int gold; // 보유 골드

    public List<string> unlockedRecipeIds;      // 해금한 레시피 ID 목록
    public List<string> unlockedIngredientIds;  // 해금한 재료 ID 목록
    public List<IngredientStock> warehouseStock; // 창고 재료별 수량

    public List<CarriageSave> carriageLevels;
    public int adUpgradeLevel;       // 광고 업그레이드 레벨

    public List<NPCUpgradeSave> npcUpgradeLevels; // 역할별 NPC 업그레이드 레벨
    public List<CarriageBoxStock> carriageBoxStock;
    public int takeoutUpgradeLevel;
    public List<TableSave> tableLevels;

    public float bgmVolume = 1f;
    public float sfxVolume = 1f;
}

