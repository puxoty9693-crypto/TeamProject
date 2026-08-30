// ScriptableObject 기획 데이터(레시피, 재료, 음식 등) 원본을 보관하고 제공
using System.Collections.Generic;

public class DataManager : MMSingleton<DataManager>
{
    public List<IngredientData> allIngredients;      // 전체 재료 데이터
    public List<RecipeData> allRecipes;               // 전체 레시피 데이터
    public List<FoodData> allFoods;                   // 전체 음식 데이터
    public List<CarriageIngredientData> carriageUpgrade;
    public List<NPCUpgradeData> npcUpgradeDataList;   // 역할별 NPC 업그레이드 데이터
    public AdData adData;                             // 광고 업그레이드 데이터
    public List<TableData> tableUpgrades;
    public TakeoutUpgradeData takeoutUpgradeData;

}
