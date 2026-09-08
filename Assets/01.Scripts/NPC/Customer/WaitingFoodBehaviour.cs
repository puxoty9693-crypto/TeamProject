using UnityEngine;

public class WaitingFoodBehaviour : CustomerBehaviour
{
    public override CustomerState State => CustomerState.WaitingFood;

    public override void Enter()
    {
        base.Enter();
    }
}
