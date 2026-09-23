using UnityEngine;

public class LeavingBehaviour : CustomerBehaviour
{
    public override CustomerState State => CustomerState.Leaving;
    [Header("Status UI")]
    [SerializeField] private Sprite paidIcon;
    [SerializeField] private Sprite angerIcon;


    public override void Enter()
    {
        base.Enter();

        switch (CustomerAgent.ExitReason)
        {
            case CustomerExitReason.Normal:
                AI.StatusUI.ShowIcon(paidIcon);
                break;
            case CustomerExitReason.PatienceOver:
                AI.StatusUI.ShowIcon(angerIcon);
                break;
            default:
                AI.StatusUI.Hide();
                break;
        }

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

    public override void Exit()
    {
        AI.StatusUI.Hide();
        base.Exit();
    }
}