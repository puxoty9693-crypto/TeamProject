using System;
using UnityEngine;

public abstract class WorkerBehaviour : MonoBehaviour
{
    public abstract WorkerRole Role { get; }
    protected Worker WorkerAgent { get; private set; }

    public Transform CurrentTarget { get; private set; }

    public bool IsActive { get; private set; }
    public bool IsArrived { get; private set; }
    public event Action<Transform> OnTargetChanged;

    public virtual void Initialize(Worker workerAgent)
    {
        WorkerAgent = workerAgent;

    }

    public virtual void Enter()
    {

        IsActive = true;
        IsArrived = false;

        SetTarget(WorkerAgent.RegisterWork.WaitingPoint);
    }

    public virtual void Exit() 
    {
        IsActive = false;
        IsArrived = false;
        CurrentTarget = null;
    }

    public virtual void Arrived()
    {
        IsArrived = true;
    }

    public virtual void Tick()
    {

    }

    protected void SetTarget(Transform target)
    {
        CurrentTarget = target;
        IsArrived = false;

        OnTargetChanged?.Invoke(target);    // 이벤트
    }

    protected void ReturnToWaitingPoint()
    {
        SetTarget(WorkerAgent.RegisterWork.WaitingPoint);
    }

}
