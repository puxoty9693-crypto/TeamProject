using UnityEngine;

public class EnteringBehaviour : CustomerBehaviour
{
    public override CustomerState State => CustomerState.Entering;

    public override void Enter()
    {
        base.Enter();
        SetTarget(AI.OrderingPoint);
    }

    public override void Arrived()
    {
        base.Arrived();
        ChangeState(CustomerState.Ordering);
    }
}
