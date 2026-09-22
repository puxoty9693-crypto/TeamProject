using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingRecipeInfo : MonoBehaviour
{
    [Header ("재료와음식정보")]
    [SerializeField] Image ingredient1Image1;
    [SerializeField] Image ingredient1Image2;
    [SerializeField] Image foodImage;
    [SerializeField] TextMeshProUGUI ingredient1Count;
    [SerializeField] TextMeshProUGUI ingredient2Count;
    

    [Header("숫자업다운")]
    [SerializeField] Button plus1Btn;
    [SerializeField] Button plus5Btn;
    [SerializeField] Button plus10Btn;
    [SerializeField] Button clearCountBtn;
    [SerializeField] TextMeshProUGUI countText;

    [Header("요리 버튼")]
    [SerializeField] Button cookingButton;
    [SerializeField] GameObject selectionPanel;

    [Header("요리 진행중")]
    [SerializeField] GameObject cookingPanel; // 취소버튼있는 판낼
    [SerializeField] TextMeshProUGUI previewTimeText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI remainingText;
    [SerializeField] Button cancelButton;

    private int currentCount = 0;
    private int maxAffordableCount = 0;
    private RecipeData currentData;

    private CraftingController Controller => GameManager.Instance.craftingController;
    private CraftingSystem System => GameManager.Instance.CraftingSystem;

    private void Awake()
    {
        plus1Btn.onClick.AddListener(() => ChangeCount(1));
        plus5Btn.onClick.AddListener(() => ChangeCount(5));
        plus10Btn.onClick.AddListener(() => ChangeCount(10));
        clearCountBtn.onClick.AddListener(ClearCount);

        cookingButton.onClick.AddListener(TryStartCooking);
        cancelButton.onClick.AddListener(() => Controller.CancelCooking());
    }
    private void OnEnable()
    {
        System.OnCookingStarted += OnCookingStarted;
        System.OnCookingCanceled += OnCookingEnded;
        System.OnCookingCompleted += OnCookingEnded;
        EventManager.Instance.AddListener(EventType.OnWarehouseChanged, OnWarehouseChanged);


        RefreshPanelByState();
    }
    private void OnDisable()
    {
        System.OnCookingStarted -= OnCookingStarted;
        System.OnCookingCanceled -= OnCookingEnded;
        System.OnCookingCompleted -= OnCookingEnded;
        if(EventManager.HasInstance)
        EventManager.Instance.RemoveListener(EventType.OnWarehouseChanged, OnWarehouseChanged);
    }
    private void Update()
    {
        if (!cookingPanel.activeSelf)
            return;
        if (!Controller.IsCooking || Controller.CookingRecipe == null)
            return;

        float remaining = Controller.CookingRecipe.cookingTime * (1f - Controller.CookingProgressRatio);
        timerText.text = $"{remaining:0.0}초";
        remainingText.text = $"남은 요리 : {Controller.RemainingCraftCount}개";
    }
    public void UpdateCookingInfo(RecipeData data)
    {
        currentData = data;
        currentCount = 1;
        maxAffordableCount = CalculateMaxAffordable(data);

        ingredient1Image1.sprite = data.food.requiredIngredients[0].ingredient.ingredientImage;
        ingredient1Image2.sprite = data.food.requiredIngredients[1].ingredient.ingredientImage;
        foodImage.sprite = data.food.foodImage;
        ingredient1Count.text = $"{data.food.requiredIngredients[0].amount}";
        ingredient2Count.text = $"{data.food.requiredIngredients[1].amount}";

        RefreshCountText();
        RefreshPanelByState();
    }
    private void ChangeCount(int count)
    {
        int newCount = Mathf.Clamp(currentCount + count, 1, maxAffordableCount);
        if (newCount == currentCount && count > 0)
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "더 만들 수 없습니다.");
            return;
        }
        currentCount = newCount;
        RefreshCountText();
    }

    public void ClearCount()
    {
        currentCount = 1;
        RefreshCountText();
    }

    private void RefreshCountText()
    {
        countText.text = $"{currentCount}";
        remainingText.text = $"남은 요리 : {Controller.RemainingCraftCount}개";
        RefreshPreviewTime();
    }
    private void RefreshPreviewTime()
    {
        if (currentData == null)
            return;

        float totalSeconds = currentData.cookingTime * currentCount;
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.FloorToInt(totalSeconds % 60f);
        previewTimeText.text = $"{minutes:00}:{seconds:00}";
    }
    private int CalculateMaxAffordable(RecipeData data)
    {
        int max = int.MaxValue;
        foreach (var req in data.food.requiredIngredients)
        {
            int owned = SaveManager.Instance.CurrentData.GetIngredientCount(req.ingredient.ingredientId);
            max = Mathf.Min(max, owned / req.amount);
        }
        return Mathf.Max(0, max);
    }
    private void TryStartCooking()
    {
        Debug.Log("🔥 TryStartCooking 호출");
        if (currentData == null)
            return;

        bool selected = Controller.SelectRecipe(currentData);

        if(!selected)
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "레시피를 선택할 수 없습니다");
            return;
        }

        Controller.AddCraftingRequest(currentCount);
        bool success = System.StartRequestedCrafting();

        if (!success)
        {
            Controller.ClearCraftingRequest();
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "재료가 부족합니다");
        }
    }

    private void OnCookingStarted(RecipeData recipe) => RefreshPanelByState();
    private void OnCookingEnded(RecipeData recipe)
    {
        ClearCount();
        RefreshPanelByState();
    }
        

    private void RefreshPanelByState()
    {
        bool isCooking = Controller.IsCooking;
        selectionPanel.SetActive(!isCooking);
        cookingPanel.SetActive(isCooking);

        SetButtonsVisible(currentData != null);
    }

    private void OnWarehouseChanged(Component sender, object param)
    {
        if (currentData == null)
            return;

        maxAffordableCount = CalculateMaxAffordable(currentData);
        currentCount = Mathf.Clamp(currentCount, currentCount == 0 ? 0 : 1, maxAffordableCount);
        ingredient1Count.text = $"{currentData.food.requiredIngredients[0].amount}";
        ingredient2Count.text = $"{currentData.food.requiredIngredients[1].amount}";
        RefreshCountText();
    }
    private void SetButtonsVisible(bool visible)
    {
        plus1Btn.gameObject.SetActive(visible);
        plus5Btn.gameObject.SetActive(visible);
        plus10Btn.gameObject.SetActive(visible);
        clearCountBtn.gameObject.SetActive(visible);
        cookingButton.gameObject.SetActive(visible);

        ingredient1Image1.gameObject.SetActive(visible);
        ingredient1Image2.gameObject.SetActive(visible);
        foodImage.gameObject.SetActive(visible);
    }
}
