using UnityEngine;

public class LeavingBehaviour : CustomerBehaviour
{
    public override CustomerState State => CustomerState.Leaving;

    public override void Enter()
    {
        base.Enter();

        CustomerAgent.EndPatience();

        SetTarget(AI.ExitPoint);
    }

    public override void Arrived()
    {
        base.Arrived();

        Debug.Log(
            $"{CustomerAgent.name} 퇴장 / {CustomerAgent.ExitReason}");

        // NPCPool.Return()
    }
}