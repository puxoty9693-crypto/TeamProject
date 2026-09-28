using UnityEngine;
using System;


public abstract class CustomerBehaviour : MonoBehaviour
{
    public abstract CustomerState State { get; }

    protected Customer CustomerAgent { get; private set; }
    protected CustomerAI AI { get; private set; }
    public Transform CurrentTarget { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsArrived { get; private set; }
    public event Action<Transform> OnTargetChanged;
    public event Action<CustomerState> OnStateChanged;

    public virtual void Initialize(Customer CustomerAgent_, CustomerAI AI_)
    {
        CustomerAgent = CustomerAgent_;
        AI = AI_;
    }

    public virtual void Enter()
    {
        IsActive = true;
        IsArrived = false;
    }

    public virtual void Exit()
    {
        IsActive = false;
        IsArrived = false;
        CurrentTarget = null;

    }

    public virtual void Tick()
    {

    }
    public virtual void Arrived()
    {
        IsArrived = true;
    }

    protected void SetTarget(Transform target)
    {
        CurrentTarget = target;
        IsArrived = false;
        OnTargetChanged?.Invoke(target);
    }
    
    protected void ChangeState(CustomerState state)
    {
        OnStateChanged?.Invoke(state);
    }

}
