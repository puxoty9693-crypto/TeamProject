using UnityEngine;

public class TestGameManager : MMSingleton<TestGameManager>
{
    [SerializeField] private CraftingController craftingController;
    private CraftingSystem craftingSystem;
    private PaymentSystem paymentSystem;
    private FoodService foodService;
    protected override void Awake()
    {
        base.Awake();
        PlayerData pData = SaveManager.Instance.CurrentData;
        paymentSystem = new PaymentSystem(pData);
        foodService = new FoodService(pData);
        craftingSystem = new CraftingSystem(pData);
        craftingController.Initialize(craftingSystem);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //캐셔NPC 에 이 paymentSystem api 받아서 사용
    public PaymentSystem GetPaymentSystem()
    {
        return paymentSystem;
    }

    //서빙NPC 에 이 foodService Api 받아서 사용
    public FoodService GetFoodService()
    {
        return foodService;
    }

}
