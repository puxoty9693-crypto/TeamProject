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

    private int currentCount = 1;
    private int maxAffordableCount = 1;
    private RecipeData currentData;

    private CraftingController Controller => TestGameManager.Instance.craftingController;
    private CraftingSystem System => TestGameManager.Instance.CraftingSystem;

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
        EventManager.Instance.AddListener(EventType.OnCookingStarted, OnCookingStarted);
        EventManager.Instance.AddListener(EventType.OnCookingCompleted, OnCookingEnded);
        EventManager.Instance.AddListener(EventType.OnCookingCanceled, OnCookingEnded);

        RefreshPanelByState();
    }
    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventType.OnCookingStarted, OnCookingStarted);
        EventManager.Instance.RemoveListener(EventType.OnCookingCompleted, OnCookingEnded);
        EventManager.Instance.RemoveListener(EventType.OnCookingCanceled, OnCookingEnded);
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
        currentCount = Mathf.Clamp(currentCount + count, 1, maxAffordableCount);
        RefreshCountText();
    }

    public void ClearCount()
    {
        currentCount = 0;
    }

    private void RefreshCountText()
    {
        countText.text = $"{currentCount}";
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
        return Mathf.Max(1, max);
    }
    private void TryStartCooking()
    {
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

    private void OnCookingStarted(Component sender, object param) => RefreshPanelByState();
    private void OnCookingEnded(Component sender, object param) => RefreshPanelByState();

    private void RefreshPanelByState()
    {
        bool isCooking = Controller.IsCooking;
        selectionPanel.SetActive(!isCooking);
        cookingPanel.SetActive(isCooking);
    }
}
