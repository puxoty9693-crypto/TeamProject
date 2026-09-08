using UnityEngine;

public class MoveToSeatBehaviour : CustomerBehaviour
{
    public override CustomerState State => CustomerState.MoveToSeat;

    public override void Enter()
    {
        base.Enter();
        if(CustomerAgent.ReservedSeat == null)
        {
            ChangeState(CustomerState.Ordering);
            return;
        }

        SetTarget(CustomerAgent.ReservedSeat);

    }

    public override void Arrived()
    {
        base.Arrived();
        Debug.Log($"{CustomerAgent.name} 도착");

        ChangeState(CustomerState.WaitingFood);
    }

}
