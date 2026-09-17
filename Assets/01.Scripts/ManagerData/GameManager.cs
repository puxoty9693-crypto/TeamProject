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

    private void Start()
    {
        PlayerData pData = SaveManager.Instance.CurrentData;

        PaymentSystem = new PaymentSystem(pData);
        FoodService = new FoodService(pData);
        CraftingSystem = new CraftingSystem(pData);
        IngredientWareHouse = new IngredientWareHouse(pData);
        ingredientUnlockSystem = new IngredientUnlockSystem(pData);
        ingredientUpgradeSystem = new IngredientUpgradeSystem(pData);

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
    }
}