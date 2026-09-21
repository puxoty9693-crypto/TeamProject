using System;
using UnityEngine;

//재료를 요리를 제외한 다른곳에서 사용하게 되면 수정해야함

/*구조 -> 요리 요청 갯수에 맞게 재료를 가지고 있어야 요리 제작 실행
 * 실제 재료 차감은 요리 완성할때마다 레시피에 1개에 맞는 재료만 차감 -> 반복
 * 취소하면 재료 남음, 요리중일때 취소하면 요리에 소모된 재료만 사라짐
*/ 

public class CraftingSystem
{
    private readonly PlayerData curData;

    // 플레이어가 현재 선택한 레시피
    public RecipeData SelectedRecipe { get; private set; }

    // 현재 제작 중인 레시피
    public RecipeData CookingRecipe { get; private set; }

    // 현재 요리공간이 사용 중인지
    public bool IsCooking { get; private set; }

    // 현재 요리 진행 시간
    public float CookingProgress { get; private set; }

    // 현재 요리의 전체 제작 시간
    private float CookingTime =>
        CookingRecipe != null
            ? CookingRecipe.cookingTime
            : 0f;

    //강화기준 제작 시간
    public float UpgradedCookingTime => 
        CookingTime != 0f ? GameManager.Instance.workerUpgradeSystem.GetUpgradedRecipeTime(CookingTime) : 0f;

    // 현재 제작 진행률
    public float CookingProgressRatio
    {
        get
        {
            if (!IsCooking || CookingTime <= 0f)
                return 0f;

            return Mathf.Clamp01(
                CookingProgress / CookingTime
            );
        }
    }

    // 현재 제작 중인 배치의 남은 제작 횟수
    public int RemainingCraftCount { get; private set; }

    // 처음 요청한 전체 제작 횟수
    public int RequestedCraftCount { get; private set; } = 0;

    // 현재까지 완료한 제작 횟수
    public int CompletedCraftCount { get; private set; }

    //제작 요청 수량이 변경될때, x1 x5 x10 버튼 눌렀을때, 제작 전
    public event Action<int> OnRequestedCraftCountChanged;
    //제작 첫 요청, 제작버튼
    public event Action<RecipeData, int> OnCraftingRequested;

    //요리가 시작될때, 매 갯수마다 반복 실행
    public event Action<RecipeData> OnCookingStarted;
    //요리가 완성될때, 매 갯수마다 반복 실행
    public event Action<RecipeData> OnCookingCompleted;
    //요리 제작을 취소할때
    public event Action<RecipeData> OnCookingCanceled;

    //제작중인 요리의 남은 수량
    public event Action<int> OnRemainingCraftCountChanged;


    public CraftingSystem(PlayerData data)
    {
        if (data == null)
        {
            Debug.LogError("PlayerData가 null입니다.");
            return;
        }

        curData = data;
    }
    public void Update(float deltaTime)
    {
        if (deltaTime < 0f)
            return;

        if (!IsCooking)
            return;

        CookingProgress += deltaTime;

        if (CookingProgress >= UpgradedCookingTime)
        {
            CompleteCooking();
        }
    }

    public void AddCraftingRequest(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("추가할 제작 수량은 1개 이상이어야 합니다.");
            return;
        }

        // 제작 중에는 요청 수량을 변경하지 못하게 함
        if (IsCooking)
        {
            Debug.LogWarning("제작 중에는 요청 수량을 변경할 수 없습니다.");
            return;
        }

        RequestedCraftCount += amount;

        OnRequestedCraftCountChanged?.Invoke(
            RequestedCraftCount
        );
    }
    public void ClearCraftingRequest()
    {
        if (IsCooking)
        {
            Debug.LogWarning("제작 중에는 요청 수량을 초기화할 수 없습니다.");
            return;
        }

        RequestedCraftCount = 0;

        OnRequestedCraftCountChanged?.Invoke(
            RequestedCraftCount
        );
    }
    public bool SelectRecipe(RecipeData recipe)
    {
        if (recipe == null)
            return false;

        if (IsCooking)
        {
            Debug.LogWarning("현재 요리 제작중이여서 선택이 불가능합니다");
            return false;
        }

        SelectedRecipe = recipe;
        return true;
    }

    // 원하는 수량만큼 제작 요청
    public bool StartRequestedCrafting()
    {
        if (IsCooking)
            return false;

        if (SelectedRecipe == null)
            return false;

        if (RequestedCraftCount <= 0)
            return false;

        if (!CanStartCooking())
        {
            Debug.LogWarning("현재 재료가 부족하여 제작할 수 없습니다.");
            return false;
        }

        if (!HasRequiredIngredientsForAmount(SelectedRecipe, RequestedCraftCount))
        {
            Debug.LogWarning("요청한 제작 수량에 필요한 재료가 부족합니다.");
            return false;
        }

        RemainingCraftCount = RequestedCraftCount;
        CompletedCraftCount = 0;

        RequestedCraftCount = 1;

        OnRequestedCraftCountChanged?.Invoke(
            RequestedCraftCount
        );

        return TryStartNextCooking();
    }

    // 현재 제작을 취소
    public bool CancelCooking()
    {
        if (!IsCooking)
        {
            Debug.LogWarning("현재 제작 중인 요리가 없습니다.");
            return false;
        }

        RecipeData canceledRecipe = CookingRecipe;

        // 현재 제작 중인 요리만 취소
        // 이미 소모된 재료는 복구하지 않음
        CookingRecipe = null;
        CookingProgress = 0f;
        IsCooking = false;

        // 남은 제작 요청도 취소
        RemainingCraftCount = 0;

        OnRemainingCraftCountChanged?.Invoke(
            RemainingCraftCount
        );

        OnCookingCanceled?.Invoke(
            canceledRecipe
        );

        return true;
    }


    public bool CanStartCooking()
    {
        if (IsCooking)
            return false;

        if (SelectedRecipe == null)
            return false;

        if (SelectedRecipe.food == null)
            return false;

        if (SelectedRecipe.cookingTime <= 0f)
            return false;

        return HasRequiredIngredients(SelectedRecipe);
    }


    // 다음 요리 1개를 시작
    private bool TryStartNextCooking()
    {
        if (IsCooking)
            return false;

        if (RemainingCraftCount <= 0)
            return false;

        if (!CanStartCooking())
        {
            RemainingCraftCount = 0;

            OnRemainingCraftCountChanged?.Invoke(
                RemainingCraftCount
            );

            return false;
        }

        if (!ConsumeIngredients(SelectedRecipe))
            return false;
        EventManager.Instance.PostNotification(EventType.OnWarehouseChanged, null);

        CookingRecipe = SelectedRecipe;
        CookingProgress = 0f;
        IsCooking = true;

        OnCookingStarted?.Invoke(
            CookingRecipe
        );

        return true;
    }


    private bool HasRequiredIngredients(RecipeData recipe)
    {
        if (recipe == null || recipe.food == null)
            return false;

        if (recipe.food.requiredIngredients == null ||
            recipe.food.requiredIngredients.Count == 0)
        {
            return false;
        }

        foreach (var requirement in recipe.food.requiredIngredients)
        {
            if (requirement == null)
                return false;

            if (requirement.ingredient == null)
                return false;

            if (requirement.amount <= 0)
                return false;

            if (!curData.HasIngredient(
                    requirement.ingredient.ingredientId,
                    requirement.amount))
            {
                return false;
            }
        }

        return true;
    }


    private bool ConsumeIngredients(RecipeData recipe)
    {
        if (recipe == null || recipe.food == null)
            return false;

        if (!HasRequiredIngredients(recipe))
            return false;

        foreach (var requirement in recipe.food.requiredIngredients)
        {
            bool success = curData.UseIngredient(
                requirement.ingredient.ingredientId,
                requirement.amount
            );

            if (!success)
                return false;
        }

        return true;
    }


    private void CompleteCooking()
    {
        RecipeData completedRecipe = CookingRecipe;

        // 완성된 음식 1개 추가
        curData.AddFood(
            completedRecipe.food.foodId,
            1
        );
        EventManager.Instance.PostNotification(EventType.OnWarehouseChanged, null);

        CompletedCraftCount++;
        RemainingCraftCount--;

        // 현재 제작 상태 초기화
        CookingRecipe = null;
        CookingProgress = 0f;
        IsCooking = false;

        OnCookingCompleted?.Invoke(
            completedRecipe
        );

        OnRemainingCraftCountChanged?.Invoke(
            RemainingCraftCount
        );


        // 남은 제작 횟수가 있으면 다음 요리 시작
        if (RemainingCraftCount > 0)
        {
            TryStartNextCooking();
        }
    }

    private bool HasRequiredIngredientsForAmount(RecipeData recipe, int craftAmount)
    {
        if (recipe == null)
            return false;

        if (recipe.food == null)
            return false;

        if (craftAmount <= 0)
            return false;

        var requirements =
            recipe.food.requiredIngredients;

        if (requirements == null ||
            requirements.Count == 0)
        {
            return false;
        }

        foreach (var requirement in requirements)
        {
            if (requirement == null)
                return false;

            if (requirement.ingredient == null)
                return false;

            if (string.IsNullOrEmpty(
                    requirement.ingredient.ingredientId))
            {
                return false;
            }

            if (requirement.amount <= 0)
                return false;

            int requiredAmount =
                requirement.amount * craftAmount;

            int currentAmount =
                curData.GetIngredientCount(
                    requirement.ingredient.ingredientId
                );

            if (currentAmount < requiredAmount)
            {
                return false;
            }
        }

        return true;
    }
}