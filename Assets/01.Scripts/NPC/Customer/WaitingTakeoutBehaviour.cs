using UnityEngine;

public class WaitingTakeoutBehaviour : CustomerBehaviour
{
    public override CustomerState State => CustomerState.WaitingTakeout;


    public override void Enter()
    {
        base.Enter();
        if(AI.TakeoutWaitingPoint == null)
        {
            return;
        }

        SetTarget(AI.TakeoutWaitingPoint);
    }


    public override void Arrived()
    {
        base.Arrived();

        Debug.Log($"{CustomerAgent.name} Takeout 대기중");


        //이하 음식 수령까지 대기
    }
}
