// 플레이어 전체 진행 상태 (JSON으로 저장/로드, SaveManager를 통해서만 접근 권장)
using System;
using System.Collections.Generic;
using UnityEngine;

// [세이브 데이터] 창고 재료 하나의 보유 수량
[System.Serializable]
public class IngredientStock
{
    public string ingredientId; // 재료 ID (IngredientData.ingredientId와 매칭)
    public int count;           // 현재 보유 수량
}

// [세이브 데이터] 특정 NPC 역할의 현재 업그레이드 레벨
[System.Serializable]
public class NPCUpgradeSave
{
    public NPCRole role; // 업그레이드 대상 역할
    public int level;    // 현재 레벨
}

// [세이브 데이터] 재료별 마차 현재 강화 레벨
[System.Serializable]
public class CarriageSave
{
    public string ingredientId;
    public int level;
}

// [세이브 데이터] 개별 테이블의 현재 레벨
[System.Serializable]
public class TableSave
{
    public string tableId;
    public int level; // 0 = 부서진 채로 방치, 1 이상 = 수리/강화된 레벨
}

// [세이브 데이터] 창고로 옮기기 전 마차 박스에 임시로 쌓인 재료
[System.Serializable]
public class CarriageBoxStock
{
    public string ingredientId;
    public int count;
}

// [세이브 데이터] 플레이어 전체 진행 상태
[System.Serializable]
public class PlayerData
{
    // ---------- 골드 (세이브 데이터) ----------
    [SerializeField] private int gold; 
    public int Gold => gold;

    public void AddGold(int amount) 
    {
        if (amount < 0) return;
        gold += amount;
    }

    //골드가 충분하면 차감하고 true, 부족하면 false
    public bool SpendGold(int amount) 
    {
        if (amount < 0 || gold < amount) return false;
        gold -= amount;
        return true;
    }
    // ---------- 완성된 요리 재고 (세이브 데이터, FoodStock 리스트) [임시 - 나중에 태경님 커밋으로 교체 예정] ----------
    [SerializeField] private List<FoodStock> foodStock = new List<FoodStock>();
    public IReadOnlyList<FoodStock> FoodStock => foodStock;

    public void AddFood(string foodId, int amount)
    {
        if (amount <= 0) return;
        var stock = foodStock.Find(s => s.foodId == foodId);
        if (stock != null) stock.count += amount;
        else foodStock.Add(new FoodStock { foodId = foodId, count = amount });
    }

    public bool UseFood(string foodId, int amount)
    {
        var stock = foodStock.Find(s => s.foodId == foodId);
        if (stock == null || stock.count < amount) return false;
        stock.count -= amount;
        return true;
    }

    public int GetFoodCount(string foodId)
    {
        var stock = foodStock.Find(s => s.foodId == foodId);
        return stock != null ? stock.count : 0;
    }

    // ---------- 해금한 레시피 (세이브 데이터) ----------
    [SerializeField] private List<string> unlockedRecipeIds = new List<string>();
    public IReadOnlyList<string> UnlockedRecipeIds => unlockedRecipeIds;

    public void UnlockedRecipe(string recipeId) 
    {
        if (!unlockedRecipeIds.Contains(recipeId))
            unlockedRecipeIds.Add(recipeId);
    }

    public bool IsRecipeUnlocked(string recipeId) => unlockedRecipeIds.Contains(recipeId);

    // ---------- 해금한 재료 (세이브 데이터) ----------
    [SerializeField] private List<string> unlockedIngredientIds = new List<string>();
    public IReadOnlyList<string> UnlockedIngredientIds => unlockedIngredientIds;

    public void UnlockedIngriedient(string ingredientId) 
    {
        if (!unlockedIngredientIds.Contains(ingredientId))
            unlockedIngredientIds.Add(ingredientId);
    }

    public bool IsIngredientUnlocked(string ingredientId) => unlockedIngredientIds.Contains(ingredientId);

    // ---------- 창고 재료 수량 (세이브 데이터, IngredientStock 리스트) ----------
    [SerializeField] private List<IngredientStock> warehouseStock = new List<IngredientStock>();
    public IReadOnlyList<IngredientStock> WarehouseStock => warehouseStock;
    public void AddIngredient(string ingredientId, int amount) 
    {
        if (amount <= 0) return;
        var stock = warehouseStock.Find(s => s.ingredientId == ingredientId);
        if (stock != null) stock.count += amount;
        else warehouseStock.Add(new IngredientStock { ingredientId = ingredientId, count = amount });
    }

    //재료가 충분하면 차감하고 true, 부족하면 false
    public bool UseIngredient(string ingredientId, int amount) 
    {
        var stock = warehouseStock.Find(s => s.ingredientId == ingredientId);
        if (stock == null || stock.count < amount) return false;
        stock.count -= amount;
        return true;
    }

    public int GetIngredientCount(string ingredientId) 
    {
        var stock = warehouseStock.Find(s => s.ingredientId ==ingredientId);
        return stock != null ? stock.count : 0;
    }

    // ---------- 마차 박스 임시 재고 (세이브 데이터) ----------
    [SerializeField] private List<CarriageBoxStock> carriageBoxStock = new List<CarriageBoxStock>();

    public void AddToCarriageBox(string ingredientId, int amount) //추가함수
    {
        if (amount <= 0) return;
        var stock = warehouseStock.Find(s => s.ingredientId == ingredientId);
        if (stock != null) stock.count += amount;
        else carriageBoxStock.Add(new CarriageBoxStock { ingredientId = ingredientId, count = amount });
    }

    public int GetCarriageBoxCount(string ingredientId) //갯수 확인함수
    {
        var box = carriageBoxStock.Find(b => b.ingredientId == ingredientId);
        return box != null ? box.count : 0;
    }

    // 박스 → 창고 이동 (NPC가 5초마다 호출하는 함수)
    public bool MoveCarriageBoxToWarehouse(string ingredientId, int amount) 
    {
        var box = carriageBoxStock.Find(b => b.ingredientId == ingredientId);
        if (box == null || box.count < amount) return false;
        box.count -= amount;
        AddIngredient(ingredientId, amount);
        return true;
    }

    // ---------- 마차(재료별) 강화 레벨 (세이브 데이터, CarriageSave 리스트) ----------
    [SerializeField] private List<CarriageSave> carriageLevels = new List<CarriageSave>();

    public int GetCarriageLevel(string ingredientId) // 확인함수
    {
        var save = carriageLevels.Find(c => c.ingredientId == ingredientId);
        return save != null ? save.level : 0;
    }

    public void UpgradeCarriageLevel(string ingredientId) // 추가(레벨업)함수
    {
        var save = carriageLevels.Find(c => c.ingredientId == ingredientId);
        if (save != null) save.level++;
        else carriageLevels.Add(new CarriageSave { ingredientId = ingredientId, level = 1 });
    }

    // ---------- NPC 업그레이드 레벨 (세이브 데이터) ----------
    [SerializeField] private List<NPCUpgradeSave> npcUpgradeLevels = new List<NPCUpgradeSave>();

    public int GetNPCUpgradeLevel(NPCRole role) // 확인함수
    {
        var save = npcUpgradeLevels.Find(n => n.role == role);
        return save != null ? save.level : 0;
    }

    public void UpgradeNPCLevel(NPCRole role) // 추가(레벨업)함수
    {
        var save = npcUpgradeLevels.Find(n  => n.role == role);
        if (save != null) save.level++;
        else npcUpgradeLevels.Add(new NPCUpgradeSave { role = role, level = 1 });
    }

    // ---------- 테이블 레벨 (세이브 데이터) ----------
    [SerializeField] private List<TableSave> tableLevels = new List<TableSave>();

    public int GetTableLevel(string tableId) // 확인함수
    {
        var save = tableLevels.Find(t => t.tableId == tableId);
        return save != null ? save.level : 0;
    }

    public void UpgradeTableLevel(string tableId) // 추가(레벨업)함수
    {
        var save = tableLevels.Find(t =>t.tableId == tableId);
        if (save != null) save.level++;
        else tableLevels.Add(new TableSave { tableId = tableId, level = 1});
    }

    // ---------- 광고 업그레이드 레벨 (세이브 데이터) ----------
    [SerializeField] private int adUpgradeLevel;
    public int AdUpgradeLevel => adUpgradeLevel;
    public void UpgradeAdLevel() => adUpgradeLevel++;

    // ---------- 테이크아웃 확률 업그레이드 레벨 (세이브 데이터) ----------
    [SerializeField] private int takeoutUpgradeLevel;
    public int TakeoutUpgradeLevel => takeoutUpgradeLevel;
    public void UpgradeTakeoutLevel() => takeoutUpgradeLevel++;

    // ---------- 사운드 볼륨 (세이브 데이터, 설정값) ----------
    [SerializeField] private float bgmVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;
    public float BgmVolume => bgmVolume;
    public float SfxVolume => sfxVolume;

    public void SetBgmVolume(float volume) => bgmVolume = Mathf.Clamp01(volume);    
    public void SetSfxVolume(float volume) => sfxVolume = Mathf.Clamp01(volume);
   
}

