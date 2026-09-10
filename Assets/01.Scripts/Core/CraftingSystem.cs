using System;
using UnityEngine;

public class CraftingSystem
{
    private readonly Inventory inventory;
    private readonly PlayerData curData;

    // 플레이어가 현재 선택한 레시피
    public RecipeData SelectedRecipe { get; private set; }

    // 현재 요리공간에서 제작 중인 레시피
    public RecipeData CookingRecipe { get; private set; }

    // 현재 요리공간이 사용 중인지
    public bool IsCooking { get; private set; }

    // 현재 요리 진행 시간
    public float CookingProgress { get; private set; }

    // 현재 요리의 전체 제작 시간
    public float CookingTime =>
        CookingRecipe != null ? CookingRecipe.cookingTime : 0f;

    // 현재 요리 진행률 (0 ~ 1)
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


    // 레시피 선택 변경
    public event Action<RecipeData> OnRecipeSelected;

    // 요리 시작
    public event Action<RecipeData> OnCookingStarted;

    // 요리 완료
    public event Action<RecipeData> OnCookingCompleted;


    public CraftingSystem(
        Inventory inventory,
        PlayerData data)
    {
        this.inventory = inventory;
        curData = data;
    }

    public void SelectRecipe(RecipeData recipe)
    {
        if (recipe == null)
        {
            Debug.LogWarning("선택하려는 레시피가 존재하지 않습니다.");
            return;
        }

        SelectedRecipe = recipe;

        OnRecipeSelected?.Invoke(recipe);

        // 현재 요리 중이 아니라면
        // 새로 선택한 레시피의 제작 가능 여부를 바로 확인한다.
        if (!IsCooking)
        {
            TryStartCooking();
        }
    }

    public void ClearRecipe()
    {
        SelectedRecipe = null;
    }

    public void Update(float deltaTime)
    {
        if (deltaTime < 0f)
            return;

        // 요리 중이 아니라면
        // 선택된 레시피를 자동으로 제작할 수 있는지 확인
        if (!IsCooking)
        {
            TryStartCooking();
            return;
        }

        // 현재 요리 진행
        CookingProgress += deltaTime;

        // 제작 완료
        if (CookingProgress >= CookingTime)
        {
            CompleteCooking();
        }
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

    private bool TryStartCooking()
    {
        if (!CanStartCooking())
            return false;

        if (!ConsumeIngredients(SelectedRecipe))
            return false;

        CookingRecipe = SelectedRecipe;

        CookingProgress = 0f;
        IsCooking = true;

        OnCookingStarted?.Invoke(CookingRecipe);

        return true;
    }

    private bool HasRequiredIngredients(RecipeData recipe)
    {
        if (recipe.ingredients == null ||
            recipe.ingredients.Length == 0)
        {
            return false;
        }

        foreach (var requirement in recipe.ingredients)
        {
            if (requirement == null)
                return false;

            if (requirement.Ingredient == null)
                return false;

            if (requirement.Amount <= 0)
                return false;

            if (!inventory.HasIngredient(
                    requirement.Ingredient,
                    requirement.Amount))
            {
                return false;
            }
        }

        return true;
    }


    /// <summary>
    /// 제작에 필요한 재료를 소비한다.
    /// </summary>
    private bool ConsumeIngredients(RecipeData recipe)
    {
        foreach (var requirement in recipe.ingredients)
        {
            if (!inventory.RemoveIngredient(
                    requirement.Ingredient,
                    requirement.Amount))
            {
                return false;
            }
        }

        return true;
    }


    /// <summary>
    /// 현재 요리를 완료한다.
    /// </summary>
    private void CompleteCooking()
    {
        RecipeData completedRecipe = CookingRecipe;

        // 음식 창고에 완성된 음식 추가
        curData.AddFood(
            completedRecipe.food.foodId,
            1
        );

        // 요리 완료 이벤트
        OnCookingCompleted?.Invoke(completedRecipe);

        // 요리공간 초기화
        CookingRecipe = null;
        CookingProgress = 0f;
        IsCooking = false;

        // 선택된 레시피가 있다면
        // 즉시 다음 요리를 시도한다.
        TryStartCooking();
    }
}