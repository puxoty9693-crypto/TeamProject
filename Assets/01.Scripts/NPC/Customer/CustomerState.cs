using UnityEngine;

public enum CustomerState
{
    Entering = 0,   // 입장
    Ordering,   // 주문 및 주문 대기
    MoveToSeat, // 자리로 이동
    WaitingFood, // 음식 대기
    Eating,     // 식사

    WaitingTakeout, // 테이크아웃 대기

    Paying,     // 결제
    Leaving,    // 이탈

}

public enum CustomerExitReason
{
    Normal,
    PatienceOver,
}
