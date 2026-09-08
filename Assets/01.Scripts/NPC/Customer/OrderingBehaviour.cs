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

        if(CustomerAgent.Data.Type == CustomerData.CustomerType.Takeout)
        {
            ChangeState(CustomerState.WaitingTakeout);
            return;
        }

        if(!AI.SeatManager.TryReserveSeat(CustomerAgent, out Transform seat))
        {
            return;
        }

        CustomerAgent.SetSeat(seat);

        Debug.Log($"{CustomerAgent.name} : {seat.name}");

        ChangeState(CustomerState.MoveToSeat);

    }


}
