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
        animator.SetBool("IsCooking", true);
        
    }
    private void HandleCookingEnded(RecipeData _)
    {
        animator.SetBool("IsCooking", false);
        
    }
  

    public override void Enter()
    {
        base.Enter();
        craftingSystem = GameManager.Instance.CraftingSystem;
        GameLogOnlyEditor.Log("1111");
        if (craftingSystem == null) return;
        GameLogOnlyEditor.Log("2222");
        craftingSystem.OnCookingStarted += HandleCookingStarted;
        craftingSystem.OnCookingCompleted += HandleCookingEnded;
        craftingSystem.OnCookingCanceled += HandleCookingEnded;


        // transform.position  정해줄 것.
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
