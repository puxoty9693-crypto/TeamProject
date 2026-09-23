using UnityEngine;

public class MoveToSeatBehaviour : CustomerBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private NPCAnimator npcAnimator;
    public override CustomerState State => CustomerState.MoveToSeat;

    public override void Enter()
    {
        base.Enter();
        if(CustomerAgent.ReservedSeat == null)
        {
            //ChangeState(CustomerState.Ordering);
            // Seat Error
            return;
        }

        SetTarget(CustomerAgent.ReservedSeat);

    }

    public override void Arrived()
    {
        base.Arrived();
        


        if(CustomerAgent.ReservedSeat != null)
        {
            npcAnimator.SetIdleFacing(CustomerAgent.ReservedSeat.localPosition.x < 0f);
        }
        GameLogOnlyEditor.Log($"{CustomerAgent.name} 도착");

        ChangeState(CustomerState.Ordering);
    }

}
