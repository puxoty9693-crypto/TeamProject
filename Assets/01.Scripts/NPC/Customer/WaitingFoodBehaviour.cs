using UnityEngine;

public class WaitingFoodBehaviour : CustomerBehaviour
{
    public override CustomerState State => CustomerState.WaitingFood;

    public override void Enter()
    {
        base.Enter();

        if(!CustomerAgent.IsPatienceActive) CustomerAgent.BeginPatience();
        Debug.Log($"음식 대기중");
    }
}
