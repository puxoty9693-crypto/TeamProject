using Unity.VisualScripting;
using UnityEngine;


//돌발 퀘스트 보상을 적용하는 부분
public class QuestBuffService
{
    private readonly PlayerData playerData;
    private readonly IngredientBoxController ingredientBoxController;
    private readonly SuddenQuestConfig config;

    //현재 활성화 된 버프
    private QuestRewardType? activeRewardType;
    private float activeValue;
    private float timeRemaining;

    public QuestBuffService(PlayerData playerData,
        PaymentSystem paymentSystem,
        IngredientSupplySystem ingredientSupplySystem,
        CraftingSystem craftingSystem,
        IngredientBoxController ingredientBoxController,
        SuddenQuestConfig config,
        QuestManager questManager)
    {
        this.playerData = playerData;
        this.ingredientBoxController = ingredientBoxController;
        this.config = config;

        questManager.OnRewardGranted += HandleRewardGranted;
        paymentSystem.OnGoldEarned += HandleGoldEarned;
        ingredientSupplySystem.OnIngredientSupplied += HandleIngredientSupplied;
        craftingSystem.OnCookingCompleted += HandleCookingCompleted;

    }

    public void Update(float deltaTime) 
    {
        if (activeRewardType == null)
            return;
        timeRemaining -= deltaTime;

        if (timeRemaining <= 0f) 
        {
            activeRewardType = null; //버프 종료
        }
    }

    private void HandleRewardGranted(QuestRewardType type, float value) 
    {
        activeRewardType = type;
        activeValue = value;
        timeRemaining = config.buffDurationSeconds;
    }

    // 수익 얻을 때마다 호출 됨
    private void HandleGoldEarned(int baseGold) 
    {
        if (activeRewardType != QuestRewardType.IncomeBonous)
            return;

        int bonus = Mathf.RoundToInt(baseGold * (activeValue / 100f));
        if (bonus <= 0)
            return;

        playerData.AddGold(bonus);
        EventManager.Instance.PostNotification(EventType.OnChangeGold, null, playerData.Gold);
    }

    // 재료 공급될 때마다 호출됨
    private void HandleIngredientSupplied( IngredientData ingredient, int baseAmount) 
    {
        if (activeRewardType != QuestRewardType.IngredientSupplyBuff)
            return;

        int bonus = Mathf.RoundToInt(baseAmount * (activeValue - 1f));
        if (bonus <= 0)
            return;

    }

    // 요리 완성될 때마다 호출됨
    private void HandleCookingCompleted(RecipeData recipe) 
    {
        if (activeRewardType != QuestRewardType.FoodProductionBUff)
            return;
        
        if (recipe == null || recipe.food == null) 
            return;

        int bonus = Mathf.RoundToInt(activeValue);
        if (bonus <= 0) 
            return;

        playerData.AddFood(recipe.food.foodId, bonus);
        EventManager.Instance.PostNotification(EventType.OnWarehouseChanged, null);
    }


}
