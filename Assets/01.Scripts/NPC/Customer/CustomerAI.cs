using System;
using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(Customer))]
[RequireComponent(typeof(AgentMovement))]
public class CustomerAI : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private TestSeatManager seatManager;

    [Header("Points")]
    [SerializeField] private Transform orderingPoint;
    [SerializeField] private Transform takeoutWaitingPoint;

    [SerializeField] private Transform exitPoint;
    [SerializeField] private Transform payingPoint;


    


    private Customer customer;
    private AgentMovement movement;

    private readonly Dictionary<CustomerState, CustomerBehaviour> behaviours = new();

    private CustomerBehaviour currentBehaviour;
    private Transform currentTarget;
    public Transform ExitPoint => exitPoint;
    public event Action<CustomerAI> OnExitComplete;
    private bool isMoving;


  
    public Customer Customer => customer;
    public TestSeatManager SeatManager => seatManager;
    public Transform OrderingPoint => orderingPoint;
    public Transform TakeoutWaitingPoint => takeoutWaitingPoint;
    public Transform PayingPoint => payingPoint;
    private void Awake()
    {
        customer = GetComponent<Customer>();
        movement = GetComponent<AgentMovement>();

        CustomerBehaviour[] foundBehaviours =
            GetComponents<CustomerBehaviour>();

        foreach (CustomerBehaviour behaviour in foundBehaviours)
        {
            behaviour.Initialize(customer, this);

            if (behaviours.ContainsKey(behaviour.State))
            {
                Debug.Log(
                    $"Customer Behaviour 중복 : {behaviour.State}",
                    gameObject);

                continue;
            }

            behaviours.Add(behaviour.State, behaviour);

            Debug.Log(
                $"Customer Behaviour 등록 : {behaviour.State}",
                gameObject);
        }
    }

    void Update()
    {
        currentBehaviour?.Tick();

        if (!isMoving) return;              // 움직이지 않을 때
        if (!movement.HasArrived()) return; // 도착 상태가 아닐 때

        movement.Stop();
        isMoving = false;

        currentBehaviour?.Arrived();
    }

    public void FoodReceived()
    {
        switch (customer.State)
        {
            case CustomerState.WaitingFood:
                customer.EndPatience();
                ChangeState(CustomerState.Eating);
                break;
            case CustomerState.WaitingTakeout:
                customer.EndPatience();
                ChangeState(CustomerState.Paying);
                break;

        }
    }

    public void EatingFinished()
    {
        if (customer.State != CustomerState.Eating) return;
        ChangeState(CustomerState.Paying);

    }

    public void PayComplete()
    {
        if (customer.State != CustomerState.Paying) return;
        customer.SetExitReason(CustomerExitReason.Normal);

        ChangeState(CustomerState.Leaving);
    }

    public void StartCustomer()
    {
        PatienceManager.instance?.Register(this);
        ChangeState(CustomerState.Entering);
    }

    private void OnDisable()
    {
        PatienceManager.instance?.Unregister(this);
        customer?.EndPatience();
    }

    public void ChangeState(CustomerState state)
    {
        Debug.Log($"ChangeState 호출 : {state}");

        DeactivateCurrentBehaviour();

        if(!behaviours.TryGetValue(state,out CustomerBehaviour behaviour))
        {
            Debug.Log($"Customer Behaviour 없음 : {state}", gameObject);

            return;
        }

        customer.SetState(state);
        currentBehaviour = behaviour;

        currentBehaviour.OnTargetChanged += HandleTargetChanged;
        currentBehaviour.OnStateChanged += ChangeState;

        currentBehaviour.Enter();
    }

    private void HandleTargetChanged(Transform target)
    {
        currentTarget = target;

        if(currentTarget == null)
        {
            movement.Stop();
            isMoving = false;
            return;
        }
        isMoving = movement.MoveTo(currentTarget.position);

    }

    public void ExitComplete()
    {
        OnExitComplete?.Invoke(this);
    }

    private void DeactivateCurrentBehaviour()
    {
        if (currentBehaviour == null) return;

        currentBehaviour.OnTargetChanged -= HandleTargetChanged;
        currentBehaviour.OnStateChanged -= ChangeState;

        currentBehaviour.Exit();

        currentBehaviour = null;
        currentTarget = null;
        isMoving = false;
    }


    internal void OnPatienceExpired()
    {
        if (customer.State == CustomerState.Leaving) return;        // 이미 떠나고있는 대상은 return

        // 인내심이 바닥났을 때만 구동
        customer.EndPatience();
        customer.SetExitReason(CustomerExitReason.PatienceOver);

        Debug.Log($"{customer.name} 인내심 종료");

        // 주문 취소는 여기에

        ChangeState(CustomerState.Leaving);

    }

    public void ReleaseSeat()
    {
        if (customer.ReservedSeat == null) return;

        if (seatManager != null) seatManager.ReleaseSeat(customer.ReservedSeat);

        customer.ClearSeat();
    }

   
}
