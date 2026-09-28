using UnityEngine;

public class EnteringBehaviour : CustomerBehaviour
{
    public override CustomerState State => CustomerState.Entering;

    public override void Enter()
    {
        base.Enter();

        if(CustomerAgent.Data == null)
        {

            // CustomerData null 
            return;
        }

        if(CustomerAgent.ReservedSeat == null)
        {
            // Seat Null
            return;
        }

        ChangeState(CustomerState.MoveToSeat);

        
    }

    public override void Arrived()
    {
        base.Arrived();
        ChangeState(CustomerState.Ordering);
    }
}
