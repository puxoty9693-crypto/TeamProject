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
    



    private Customer customer;
    private AgentMovement movement;

    private readonly Dictionary<CustomerState, CustomerBehaviour> behaviours = new();

    private CustomerBehaviour currentBehaviour;
    private Transform currentTarget;

    private bool isMoving;


  
    public Customer Customer => customer;
    public TestSeatManager SeatManager => seatManager;
    public Transform OrderingPoint => orderingPoint;
    public Transform TakeoutWaitingPoint => takeoutWaitingPoint;

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

    public void StartCustomer()
    {
        ChangeState(CustomerState.Entering);

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
        throw new NotImplementedException();
    }

   
}
