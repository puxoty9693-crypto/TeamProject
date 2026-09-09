using UnityEngine;

public class OrderingBehaviour : CustomerBehaviour
{
    public override CustomerState State => CustomerState.Ordering;

    private const float SeatCheckInterval = 0.2f;   // 좌석 확인 딜레이

    private float nextSeatCheckTime;

    public override void Enter()
    {
        base.Enter();
        nextSeatCheckTime = 0f;
        TryReserveSeat();
    }

    public override void Tick()
    {
        if (CustomerAgent.ReservedSeat != null) return;
        if (Time.time < nextSeatCheckTime) return;

        TryReserveSeat();
    }

    private void TryReserveSeat()
    {
        nextSeatCheckTime = Time.time + SeatCheckInterval;

        if (CustomerAgent.Data == null) return;


        // Data.Type에 의거 테이크아웃 상태 진입
        if(CustomerAgent.Data.Type == CustomerData.CustomerType.Takeout)
        {
            ChangeState(CustomerState.WaitingTakeout);
            return;
        }

        // 자리가 없을 때 인내심 소모 시작
        if(!AI.SeatManager.TryReserveSeat(CustomerAgent, out Transform seat))
        {
            if (CustomerAgent.IsPatienceActive) CustomerAgent.BeginPatience();
            return;
        }

        // 자리 찾음.
        CustomerAgent.EndPatience();

        CustomerAgent.SetSeat(seat);

        Debug.Log($"{CustomerAgent.name} : {seat.name}");

        ChangeState(CustomerState.MoveToSeat);

    }


}
