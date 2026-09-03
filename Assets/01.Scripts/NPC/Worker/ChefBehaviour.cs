using UnityEngine;

public class ChefBehaviour : WorkerBehaviour
{
    public override WorkerRole Role => WorkerRole.Chef;

    public bool IsReady => IsActive && IsArrived;

    public override void Enter()
    {
        base.Enter();

        // transform.position  정해줄 것.
    }

    public override void Arrived()
    {
        base.Arrived();
        // 애니메이션 시작 등
    }

}
