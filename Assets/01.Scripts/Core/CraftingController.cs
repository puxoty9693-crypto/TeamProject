using UnityEngine;

//값 변경 업데이트 Action은 craftingSystem 에 있음
public class CraftingController : MonoBehaviour
{
    private CraftingSystem craftingSystem;

    public int RemainingCraftCount =>
    craftingSystem?.RemainingCraftCount ?? 0;

    public int RequestedCraftCount =>
        craftingSystem?.RequestedCraftCount ?? 0;

    public int CompletedCraftCount =>
        craftingSystem?.CompletedCraftCount ?? 0;

    public RecipeData SelectedRecipe =>
        craftingSystem?.SelectedRecipe;

    public RecipeData CookingRecipe =>
        craftingSystem?.CookingRecipe;

    public bool IsCooking =>
        craftingSystem != null && craftingSystem.IsCooking;

    public float CookingProgressRatio =>
        craftingSystem?.CookingProgressRatio ?? 0f;


    public void Initialize(CraftingSystem system)
    {
        if (system == null)
        {
            Debug.LogError("CraftingSystem이 null입니다.");
            return;
        }

        craftingSystem = system;
    }

    private void Update()
    {
        if (craftingSystem == null)
            return;

        craftingSystem.Update(Time.deltaTime);
    }

    //갯수 추가 버튼
    public void AddCraftingRequest(int amount)
    {
        if (craftingSystem == null)
        {
            Debug.LogWarning("CraftingSystem이 초기화되지 않았습니다.");
            return;
        }

        craftingSystem.AddCraftingRequest(amount);
    }

    //제작 버튼
    public void StartRequestedCrafting()
    {
        if (craftingSystem == null)
        {
            Debug.LogWarning("CraftingSystem이 초기화되지 않았습니다.");
            return;
        }

        craftingSystem.StartRequestedCrafting();
    }
    public bool SelectRecipe(RecipeData recipe)
    {
        if (craftingSystem == null)
        {
            Debug.LogWarning("CraftingSystem이 초기화되지 않았습니다.");
            return false;
        }
        return craftingSystem.SelectRecipe(recipe);
    }
    //갯수 초기화 버튼
    public void ClearCraftingRequest()
    {
        if (craftingSystem == null)
            return;

        craftingSystem.ClearCraftingRequest();
    }

    //요리 취소 버튼
    public void CancelCooking()
    {
        if (craftingSystem == null)
        {
            Debug.LogWarning("CraftingSystem이 초기화되지 않았습니다.");
            return;
        }

        craftingSystem.CancelCooking();
    }


}