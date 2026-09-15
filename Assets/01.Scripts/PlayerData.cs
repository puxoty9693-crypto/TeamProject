// 플레이어 전체 진행 상태 (JSON으로 저장/로드, SaveManager를 통해서만 접근 권장)
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// [세이브 데이터] 창고 재료 하나의 보유 수량
[System.Serializable]
public class IngredientStock
{
    public string ingredientId; // 재료 ID (IngredientData.ingredientId와 매칭)
    public int count;           // 현재 보유 수량
}

// [세이브 데이터] 음식 창고에 보관 중인 음식 하나의 수량
[System.Serializable]
public class FoodStock
{
    public string foodId;
    public int count;
}

// [세이브 데이터] 특정 직원 역할의 현재 업그레이드 레벨
[System.Serializable]
public class WorkerUpgradeSave
{
    public WorkerRole role; // 업그레이드 대상 역할
    public int level;    // 현재 레벨
}

// [세이브 데이터] 재료별 마차 현재 강화 레벨
[System.Serializable]
public class CarriageSave
{
    public string ingredientId;
    public int level;
}

// [세이브 데이터] 배치된 오브젝트 하나의 정보
[System.Serializable]
public class PlacedObjectSave
{
    public string objId;       // 오브젝트 타입
    public string instanceId;  // 개별 인스턴스 id (테이블만 유의미, 나머진 GUID)
    public int gridX;
    public int gridY;
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

    // 골드가 충분하면 차감하고 true, 부족하면 false
    public bool SpendGold(int amount)
    {
        if (amount < 0 || gold < amount) return false;
        gold -= amount;
        return true;
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

    public void UnlockIngredient(string ingredientId)
    {
        if (!unlockedIngredientIds.Contains(ingredientId))
            unlockedIngredientIds.Add(ingredientId);
    }

    public bool IsIngredientUnlocked(string ingredientId) => unlockedIngredientIds.Contains(ingredientId);

    // ---------- 재료 창고 (세이브 데이터, IngredientStock 리스트) ----------
    [SerializeField] private List<IngredientStock> warehouseStock = new List<IngredientStock>();
    public IReadOnlyList<IngredientStock> WarehouseStock => warehouseStock;

    public void AddIngredient(string ingredientId, int amount)
    {
        if (amount <= 0) return;
        var stock = warehouseStock.Find(s => s.ingredientId == ingredientId);
        if (stock != null) stock.count += amount;
        else warehouseStock.Add(new IngredientStock { ingredientId = ingredientId, count = amount });
    }

    // 재료가 충분하면 차감하고 true, 부족하면 false
    public bool UseIngredient(string ingredientId, int amount)
    {
        var stock = warehouseStock.Find(s => s.ingredientId == ingredientId);
        if (stock == null || stock.count < amount) return false;
        stock.count -= amount;
        return true;
    }

    // 지금 몇 개 있어?
    public int GetIngredientCount(string ingredientId)
    {
        var stock = warehouseStock.Find(s => s.ingredientId == ingredientId);
        return stock != null ? stock.count : 0;
    }

    // N개 이상 있어? (있으면 Yes, 없으면 No)
    public bool HasIngredient(string ingredientId, int amount)
        => GetIngredientCount(ingredientId) >= amount;

    // ---------- 음식 창고 (세이브 데이터, FoodStock 리스트) ----------
    [SerializeField] private List<FoodStock> foodStock = new List<FoodStock>();
    public IReadOnlyList<FoodStock> FoodStock => foodStock;

    public void AddFood(string foodId, int amount)
    {
        if (amount <= 0) return;
        var stock = foodStock.Find(f => f.foodId == foodId);
        if (stock != null) stock.count += amount;
        else foodStock.Add(new FoodStock { foodId = foodId, count = amount });
    }

    // 음식이 충분하면 차감하고 true, 부족하면 false
    public bool UseFood(string foodId, int amount)
    {
        var stock = foodStock.Find(f => f.foodId == foodId);
        if (stock == null || stock.count < amount) return false;
        stock.count -= amount;
        return true;
    }

    // 지금 몇 개 있는지 조회(없으면 0)
    public int GetFoodCount(string foodId)
    {
        var stock = foodStock.Find(f => f.foodId == foodId);
        return stock != null ? stock.count : 0;
    }

    // N개 이상 있어? (있으면 Yes, 없으면 No)
    public bool HasFood(string foodId, int amount)
        => GetFoodCount(foodId) >= amount;

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

    // ---------- 직원 업그레이드 레벨 (세이브 데이터) ----------
    [SerializeField] private List<WorkerUpgradeSave> workerUpgradeLevels = new List<WorkerUpgradeSave>();

    public int GetWorkerUpgradeLevel(WorkerRole role) // 확인함수
    {
        var save = workerUpgradeLevels.Find(w => w.role == role);
        return save != null ? save.level : 0;
    }

    public void UpgradeWorkerLevel(WorkerRole role) // 추가(레벨업)함수
    {
        var save = workerUpgradeLevels.Find(w => w.role == role);
        if (save != null) save.level++;
        else workerUpgradeLevels.Add(new WorkerUpgradeSave { role = role, level = 1 });
    }

    // ---------- 광고 업그레이드 레벨 (세이브 데이터) ----------
    [SerializeField] private int adUpgradeLevel;
    public int AdUpgradeLevel => adUpgradeLevel;
    public void UpgradeAdLevel() => adUpgradeLevel++;

    // ---------- 사운드 볼륨 (세이브 데이터, 설정값) ----------
    [SerializeField] private float bgmVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;
    public float BgmVolume => bgmVolume;
    public float SfxVolume => sfxVolume;

    public void SetBgmVolume(float volume) => bgmVolume = Mathf.Clamp01(volume);
    public void SetSfxVolume(float volume) => sfxVolume = Mathf.Clamp01(volume);

    // ---------- 테이블 업그레이드 레벨 (세이브 데이터, 글로벌) 경은 추가 ----------
    [SerializeField] private int tableUpgradeLevel;
    public int TableUpgradeLevel => tableUpgradeLevel;
    public void UpgradeTableLevel() => tableUpgradeLevel++;

    // ---------- 하우징 배치 (세이브 데이터) ----------
    [SerializeField] private List<PlacedObjectSave> placedObjects = new List<PlacedObjectSave>();
    public IReadOnlyList<PlacedObjectSave> PlacedObjects => placedObjects;

    [SerializeField] private int nextTableIndex = 0; // 순차 id 카운터, 테이블 전용

    public void AddPlacedObject(string objId, string instanceId, Vector2Int gridPos)
    {
        placedObjects.Add(new PlacedObjectSave { objId = objId, instanceId = instanceId, gridX = gridPos.x, gridY = gridPos.y });
    }

    public void UpdatePlacedObjectPosition(string instanceId, Vector2Int gridPos)
    {
        var save = placedObjects.Find(p => p.instanceId == instanceId);
        if (save != null)
        {
            save.gridX = gridPos.x;
            save.gridY = gridPos.y;
        }
    }

    public void RemovePlacedObject(string instanceId)
    {
        placedObjects.RemoveAll(p => p.instanceId == instanceId);
    }

    public string GetNextTableId()
    {
        string id = $"table_{nextTableIndex}";
        nextTableIndex++;
        return id;
    }
}