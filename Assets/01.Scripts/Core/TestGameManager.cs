using UnityEngine;

//박스 옮기는 NPC IngredientWareHouse 받기
//캐셔 NPC PaymentSystem 받기
//서빙 NPC FoodService 받기
public class TestGameManager : MMSingleton<TestGameManager>
{
    public CraftingController craftingController;
    public CarriageController carriageController;
    public IngredientBoxController ingredientBoxController;
    public RecipeUnlockController recipeUnlockController;
    private CraftingSystem craftingSystem;
    private PaymentSystem paymentSystem;
    private FoodService foodService;
    private IngredientSupplySystem ingredientSupplySystem;
    private IngredientWareHouse ingredientWareHouse;
    private RecipeUnlockSystem recipeUnlockSystem;
    protected override void Awake()
    {
        base.Awake();
        PlayerData pData = SaveManager.Instance.CurrentData;
        paymentSystem = new PaymentSystem(pData);
        foodService = new FoodService(pData);
        craftingSystem = new CraftingSystem(pData);
        ingredientWareHouse = new IngredientWareHouse(pData);
        ingredientSupplySystem = new IngredientSupplySystem(pData, DataManager.Instance.allIngredients, ingredientBoxController);
        recipeUnlockSystem = new RecipeUnlockSystem(pData);
        recipeUnlockController.Initialize(recipeUnlockSystem);
        craftingController.Initialize(craftingSystem);
        carriageController.Initialize(ingredientSupplySystem);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public IngredientWareHouse GetIngredientWareHouse()
    {
        return ingredientWareHouse;
    }
    public IngredientSupplySystem GetIngredientSupplySystem()
    {
        return ingredientSupplySystem;
    }

    public IngredientBoxController GetIngredientBox()
    {
        return ingredientBoxController;
    }

    public PaymentSystem GetPaymentSystem()
    {
        return paymentSystem;
    }

    public FoodService GetFoodService()
    {
        return foodService;
    }

}
