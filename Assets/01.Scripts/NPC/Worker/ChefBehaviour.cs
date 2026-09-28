using UnityEngine;

public class ChefBehaviour : WorkerBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Color cookingColor = Color.green;

    
    private CraftingSystem craftingSystem;
    private GameManager gameManager;
    private bool isCooking;
    public override WorkerRole Role => WorkerRole.Chef;

    public bool IsReady => IsActive && IsArrived;

    

    private void HandleCookingStarted(RecipeData recipe)
    {

        GameLogOnlyEditor.Log("Cooking Start");
        animator.SetBool("IsCooking", true);

        AI.StatusUI.ShowProgress(recipe.food.foodImage, GetTotalProgress(), cookingColor);
        
    }
    private void HandleCookingEnded(RecipeData _)
    {
        GameLogOnlyEditor.Log("Cooking Finish");
        animator.SetBool("IsCooking", false);
        AI.StatusUI.Hide();
        
    }

    private float GetTotalProgress()
    {
        int total = craftingSystem.CompletedCraftCount + craftingSystem.RemainingCraftCount;

        if (total <= 0) return 0;

        return (craftingSystem.CompletedCraftCount + craftingSystem.CookingProgressRatio) / total;
    }
  

    public override void Enter()
    {
        base.Enter();

        AI.StatusUI.Hide();
        TryGetCraftingSystem();


        
    }
    private void TryGetCraftingSystem()
    {
        if (GameManager.Instance.CraftingSystem == null) return;

        craftingSystem = GameManager.Instance.CraftingSystem;

        craftingSystem.OnCookingStarted += HandleCookingStarted;
        craftingSystem.OnCookingCompleted += HandleCookingEnded;
        craftingSystem.OnCookingCanceled += HandleCookingEnded;

        if(craftingSystem.IsCooking && craftingSystem.CookingRecipe != null)
        {
            HandleCookingStarted(craftingSystem.CookingRecipe);
        }

        GameLogOnlyEditor.Log("craftingSystem Loaded");
    }
    public override void Tick()
    {
        if(craftingSystem == null)
        {
            TryGetCraftingSystem();
            return;
        }
        if (!craftingSystem.IsCooking) return;

        AI.StatusUI.SetProgress(GetTotalProgress(), cookingColor);
        
            
    }

    public override void Arrived()
    {
        base.Arrived();
        
     
    }

    public override void Exit()
    {
        if(craftingSystem != null)
        {
            craftingSystem.OnCookingStarted -= HandleCookingStarted;
            craftingSystem.OnCookingCompleted -= HandleCookingEnded;
            craftingSystem.OnCookingCanceled -= HandleCookingEnded;

            craftingSystem = null;
            
        }
        animator.SetBool("IsCooking", false);
        AI.StatusUI.Hide();
        base.Exit();
        
    }
    
}
