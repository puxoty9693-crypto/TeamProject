using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MMSingleton<GameManager>
{
    public CraftingController craftingController;
    public CarriageController carriageController;
    public IngredientBoxController ingredientBoxController;
    public RecipeUnlockController recipeUnlockController;

    public CraftingSystem CraftingSystem { get; private set; }
    public PaymentSystem PaymentSystem { get; private set; }
    public FoodService FoodService { get; private set; }
    public IngredientSupplySystem IngredientSupplySystem { get; private set; }
    public IngredientWareHouse IngredientWareHouse { get; private set; }
    public RecipeUnlockSystem RecipeUnlockSystem { get; private set; }
    public IngredientUnlockSystem ingredientUnlockSystem { get; private set; }
    public IngredientUpgradeSystem ingredientUpgradeSystem { get; private set; }
    public WorkerUpgradeSystem workerUpgradeSystem { get; private set; }

    public QuestBuffService QuestBuffService { get; private set; }

    public QuestManager QuestManager { get; private set; }

    private void Start()
    {
        PlayerData pData = SaveManager.Instance.CurrentData;

        PaymentSystem = new PaymentSystem(pData);
        FoodService = new FoodService(pData);
        CraftingSystem = new CraftingSystem(pData);
        IngredientWareHouse = new IngredientWareHouse(pData);
        ingredientUnlockSystem = new IngredientUnlockSystem(pData);
        ingredientUpgradeSystem = new IngredientUpgradeSystem(pData);
        QuestManager = new QuestManager(pData, DataManager.Instance.suddenQuestConfig);
        workerUpgradeSystem = new WorkerUpgradeSystem(pData);

        IngredientSupplySystem =
            new IngredientSupplySystem(
                pData,
                DataManager.Instance.allIngredients,
                ingredientBoxController
            );

        RecipeUnlockSystem = new RecipeUnlockSystem(pData);

        recipeUnlockController.Initialize(RecipeUnlockSystem);
        craftingController.Initialize(CraftingSystem);
        carriageController.Initialize(IngredientSupplySystem);

        QuestBuffService = new QuestBuffService(
            pData,
            PaymentSystem,
            IngredientSupplySystem,
            CraftingSystem,
            ingredientBoxController,
            DataManager.Instance.suddenQuestConfig,
            QuestManager
            );

    }

    private void Update()
    {
        QuestManager.Update(Time.deltaTime);
        QuestBuffService.Update(Time.deltaTime);
        // 임시 종료 가능 나중에 종료 확인 팝업 등으로 교체 예정
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) 
        {
            QuitGame();
        }
    }
    
    private void QuitGame() 
    {
        Application.Quit();
    }

}

