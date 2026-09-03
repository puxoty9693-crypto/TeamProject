using System;
using UnityEngine;

public class Worker : MonoBehaviour
{
    [SerializeField] private WorkerRegisterWork registerWork = new();

    public WorkerData Data { get; private set; }
    public WorkerStats Stats { get; private set; }
    public WorkerRegisterWork RegisterWork => registerWork;
    public event Action<WorkerRegisterWork> OnWorkerRoleChanged;

    public void Initialize(WorkerData data) // 각 worker 초기화
    {
        if (data == null) return;

        Data = data;
        Stats = data.BaseStats.Clone();
    }

    public void Register(WorkerRole role, Transform waitingPoint)
    {
        registerWork.Register(role, waitingPoint);
        OnWorkerRoleChanged?.Invoke(registerWork);
    }

    public void ClearRole()
    {
        registerWork.Clear();
        OnWorkerRoleChanged?.Invoke(registerWork);
    }

}
