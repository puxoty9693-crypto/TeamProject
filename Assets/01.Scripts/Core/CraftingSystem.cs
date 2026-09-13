using System;
using UnityEngine;

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
    public float CookingTime =>
        CookingRecipe != null
            ? CookingRecipe.cookingTime
            : 0f;

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
    public int RequestedCraftCount { get; private set; }

    // 현재까지 완료한 제작 횟수
    public int CompletedCraftCount { get; private set; }


    // 이벤트
    public event Action<RecipeData> OnRecipeSelected;
    public event Action<RecipeData, int> OnCraftingRequested;

    public event Action<RecipeData> OnCookingStarted;
    public event Action<RecipeData> OnCookingCompleted;
    public event Action<RecipeData> OnCookingCanceled;

    // 제작 수량이 변경되었을 때 사용
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


    // 레시피 선택만 담당
    public void SelectRecipe(RecipeData recipe)
    {
        if (recipe == null)
        {
            Debug.LogWarning("선택하려는 레시피가 존재하지 않습니다.");
            return;
        }

        SelectedRecipe = recipe;

        OnRecipeSelected?.Invoke(recipe);
    }


    public void ClearRecipe()
    {
        if (IsCooking)
        {
            Debug.LogWarning("제작 중에는 레시피를 해제할 수 없습니다.");
            return;
        }

        SelectedRecipe = null;
    }


    // 원하는 수량만큼 제작 요청
    public bool RequestCrafting(int count)
    {
        if (SelectedRecipe == null)
        {
            Debug.LogWarning("먼저 레시피를 선택해야 합니다.");
            return false;
        }

        if (count <= 0)
        {
            Debug.LogWarning("제작 수량은 1개 이상이어야 합니다.");
            return false;
        }

        // 이미 제작 요청이 있는 경우
        if (IsCooking || RemainingCraftCount > 0)
        {
            Debug.LogWarning("이미 제작 요청이 진행 중입니다.");
            return false;
        }

        RequestedCraftCount = count;
        CompletedCraftCount = 0;
        RemainingCraftCount = count;

        OnCraftingRequested?.Invoke(
            SelectedRecipe,
            count
        );

        // 첫 번째 제작 시작
        TryStartNextCooking();

        return true;
    }


    public void Update(float deltaTime)
    {
        if (deltaTime < 0f)
            return;

        if (!IsCooking)
            return;

        CookingProgress += deltaTime;

        if (CookingProgress >= CookingTime)
        {
            CompleteCooking();
        }
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

            if (curData.HasIngredient(
                    requirement.ingredient.ingredientId, requirement.amount))
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

        CompletedCraftCount++;
        RemainingCraftCount--;

        OnCookingCompleted?.Invoke(
            completedRecipe
        );

        OnRemainingCraftCountChanged?.Invoke(
            RemainingCraftCount
        );

        // 현재 제작 상태 초기화
        CookingRecipe = null;
        CookingProgress = 0f;
        IsCooking = false;

        // 남은 제작 횟수가 있으면 다음 요리 시작
        if (RemainingCraftCount > 0)
        {
            TryStartNextCooking();
        }
    }
}