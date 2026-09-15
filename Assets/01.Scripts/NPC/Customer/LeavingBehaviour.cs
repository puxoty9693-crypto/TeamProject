using UnityEngine;

public class LeavingBehaviour : CustomerBehaviour
{
    public override CustomerState State => CustomerState.Leaving;

    public override void Enter()
    {
        base.Enter();

        CustomerAgent.EndPatience();
        AI.ReleaseSeat();

        if (AI.ExitPoint == null) return;

        SetTarget(AI.ExitPoint);
    }

    public override void Arrived()
    {
        base.Arrived();

        Debug.Log(
            $"{CustomerAgent.name} 퇴장 / {CustomerAgent.ExitReason}");

        AI.ExitComplete();
        // NPCPool.Return()
    }
}