// ScriptableObject 기획 데이터(레시피, 재료, 음식 등) 원본을 보관하고 제공
using System.Collections.Generic;

public class DataManager : MMSingleton<DataManager>
{
    public List<IngredientData> allIngredients;      // 전체 재료 데이터
    public List<RecipeData> allRecipes;               // 전체 레시피 데이터
    public List<FoodData> allFoods;                   // 전체 음식 데이터
    public List<CarriageIngredientData> carriageUpgrades;     // 마차 업그레이드 데이터
    public List<WorkerData> allWorkers;
    public List<WorkerUpgradeData> workerUpgradeDataList;   // 역할별 직원 업그레이드 데이터
    public AdData adData;                             // 광고 업그레이드 데이터
    public CookingBatchConfig cookingBatchConfig;      // 제작 단위
    public TableUpgradeData tableUpgradeData;          // 테이블 업그레이드 데이터
    public SuddenQuestConfig suddenQuestConfig; // 돌발 퀘스트 데이터
    public AchievementConfig achievementConfig; // 업적 퀘스트 데이터
    //========= 조회 함수 ========

    // id로 재료 원본 데이터 찾기
    public IngredientData GetIngredient(string ingredientId)
        => allIngredients.Find(i => i.ingredientId== ingredientId);

    // id로 음식 원본 데이터 찾기
    public FoodData GetFood(string foodId)
        => allFoods.Find(f => f.foodId== foodId);
    
    // id로 레시피 원본 데이터 찾기
    public RecipeData GetRecipe(string recipeId)
        => allRecipes.Find(r => r.recipeId ==recipeId);
    
    // id로 마차 강화 데이터 찾기
    public CarriageIngredientData GetCarriageData(string ingredientId)
        => carriageUpgrades.Find(c => c.ingredient.ingredientId == ingredientId);

    // IngredientData로 마차 강화 데이터 찾기 (오버로드)
    public CarriageIngredientData GetCarriageData(IngredientData ingredient)
        => GetCarriageData(ingredient.ingredientId);

    // 역할로 직원 원본 데이터 찾기
   // public WorkerData GetWorker(WorkerRole role)
   //     => allWorkers.Find(w => w.role == role);
    
    // 역할로 직원 업그레이드 데이터 찾기
    public WorkerUpgradeData GetWorkerUpgradeData(WorkerRole role)
        => workerUpgradeDataList.Find(w => w.role == role);

}
