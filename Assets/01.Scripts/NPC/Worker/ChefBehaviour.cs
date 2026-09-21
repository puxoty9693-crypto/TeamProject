using UnityEngine;

public class ChefBehaviour : WorkerBehaviour
{
    [SerializeField] private Animator animator;

    private CraftingSystem craftingSystem;
    private GameManager gameManager;
    private bool isCooking;
    public override WorkerRole Role => WorkerRole.Chef;

    public bool IsReady => IsActive && IsArrived;



    private void HandleCookingStarted(RecipeData _)
    {

        GameLogOnlyEditor.Log("Cooking Start");
        animator.SetBool("IsCooking", true);
        
    }
    private void HandleCookingEnded(RecipeData _)
    {
        GameLogOnlyEditor.Log("Cooking Finish");
        animator.SetBool("IsCooking", false);
        
    }
  

    public override void Enter()
    {
        base.Enter();
        GameLogOnlyEditor.Log("1111");
        TryGetCraftingSystem();


        
    }
    private void TryGetCraftingSystem()
    {
        if (GameManager.Instance.CraftingSystem == null) return;

        craftingSystem = GameManager.Instance.CraftingSystem;

        craftingSystem.OnCookingStarted += HandleCookingStarted;
        craftingSystem.OnCookingCompleted += HandleCookingEnded;
        craftingSystem.OnCookingCanceled += HandleCookingEnded;
        GameLogOnlyEditor.Log("craftingSystem Loaded");
    }
    public override void Tick()
    {
        if(craftingSystem == null)
        {
            TryGetCraftingSystem();
        }
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

            
        }
        animator.SetBool("IsCooking", false);
        base.Exit();
        
    }
    
}
