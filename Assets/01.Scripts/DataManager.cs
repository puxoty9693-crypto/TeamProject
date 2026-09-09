// ScriptableObject 기획 데이터(레시피, 재료, 음식 등) 원본을 보관하고 제공
using System.Collections.Generic;

public class DataManager : MMSingleton<DataManager>
{
    public List<IngredientData> allIngredients;      // 전체 재료 데이터
    public List<RecipeData> allRecipes;               // 전체 레시피 데이터
    public List<FoodData> allFoods;                   // 전체 음식 데이터
    public List<CarriageIngredientData> carriageUpgrades;     //마차 업그레이드 데이터
    public List<WorkerData> allWorkers;
    public List<WorkerUpgradeData> workerUpgradeDataList;   // 역할별 직원 업그레이드 데이터
    public AdData adData;                             // 광고 업그레이드 데이터
    public List<TableData> tableUpgrades;            //테이블 업그레이드 데이터 
    public TakeoutUpgradeData takeoutUpgradeData;    // 픽업 업그레이드 데이터

}
