using UnityEngine;

/// <summary>
/// 
/// Customer.cs
/// Each Customer Data
/// 
/// </summary>
[RequireComponent(typeof(AgentMovement))]
//[RequireComponent(typeof(CustomerAI))] // 예비
public class Customer : MonoBehaviour
{
    public CustomerData Data { get; private set; }
    public CustomerState State { get; private set; }
    public CustomerExitReason ExitReason { get; private set; }
    //public SeatSlot CurrentSeat { get; private set; }     가칭 => 좌석 타겟팅
    public CustomerModifier Modifier { get; private set; }
    public CustomerTraitSet Traits { get; private set; }
    public AgentMovement Movement { get; private set; }

    //public CustomerAI AI { get; private set; }

    //private SeatManager seatManager;      가칭 => 좌석 관리

    private bool isPatienceActive;
    public bool IsPatienceActive => isPatienceActive;

    private float patienceMax;
    private float patienceEndTime;

    private void Awake()
    {
        Movement = GetComponent<AgentMovement>();
        //AI = GetComponent<CustomerAI>();
    }

    /// <summary>
    /// 
    /// Customer Data Init_
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void Initialize(CustomerData data)
    {
        ResetRun();
        Data = data;
        Movement.SetSpeed(data.MoveSpeed);
    }


    /// <summary>
    /// 
    /// Trying to add Trait
    /// Trait 추가 시도
    /// 
    /// </summary>
    /// <param name="trait"></param>
    /// <returns></returns>
    public bool TryAddTrait(CustomerTraitData trait)
    {
        if (!Traits.TryAdd(trait)) return false;    // add 실패 시 false 반환

        
        Traits.ApplyModifiers(Modifier);    // 정상 추가
        return true;
    }


    /// <summary>
    /// 
    /// Customer Status
    /// 
    /// </summary>
    /// <param name="state"></param>
    public void SetState(CustomerState state)
    {
        State = state;
    }
    public void SetExitReason(CustomerExitReason exitReason)
    {
        ExitReason = exitReason;
    }

    public void SetModifier(CustomerModifier modifier)
    {
        Modifier = modifier;
    }

    /// <summary>
    /// for Seat
    /// </summary>
    public void ReserveSeat()
    {

    }

    public void ReleaseSeat()
    {

    }

    /// <summary>
    /// 
    /// for Patience
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetMaxPatience()
    {
        if (Data == null) return 0f;

        return Data.BasePatience * Modifier.PatienceMultiplier;
    }

    public void BeginPatience()
    {
        patienceMax = GetMaxPatience();
        patienceEndTime = Time.time + patienceMax;
        isPatienceActive = true;
    }

    public void EndPatience()
    {
        isPatienceActive = false;
    }

    
    // 잔여 인내심 계산
    // For System Calculating
    public float RemainingPatience
    {
        get
        {
            if (!isPatienceActive) return patienceMax;

            return Mathf.Max(0f, patienceEndTime - Time.time);
        }
    }

    // For Gauge UI
    public float PatienceNormalized
    {
        get 
        {
            if (patienceMax <= 0f) return 0f;

            return Mathf.Clamp01(RemainingPatience / patienceMax);
            
        }
    }


    

    /// <summary>
    /// For Pooling
    /// 풀로 돌아갈 때 개별 data 초기화
    /// </summary>
    public void ResetRun()
    {

        // 이하는 Seat 스크립트 필요. 할당한 자리 반환 및 인내심 초기화
        //ReleaseSeat();
        //EndPatience();

        ExitReason = CustomerExitReason.Normal;

        // 이하는 null check 및 trait data 초기화
        Traits?.Clear();
        Modifier?.Reset();
    }

}
