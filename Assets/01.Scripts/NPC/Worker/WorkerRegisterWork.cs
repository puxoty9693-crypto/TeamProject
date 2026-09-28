using System;
using UnityEngine;


[Serializable]
public class WorkerRegisterWork
{
    [SerializeField] private WorkerRole role = WorkerRole.None; // 기본 직업 None

    [SerializeField] private Transform nullWaitingPoint;    // 기본 대기 장소
    private Transform waitingPoint;
    public WorkerRole Role => role;
    public Transform WaitingPoint => waitingPoint;

    /// <summary>
    /// 직업이 있고, 대기 장소도 존재할 때 isRegistered == true
    /// </summary>
    public bool IsRegistered => role != WorkerRole.None && waitingPoint != null;

    /// <summary>
    /// new Role Register
    /// </summary>
    /// <param name="newRole"></param>
    /// <param name="newWaitingPoint"></param>
    public void Register(WorkerRole newRole, Transform newWaitingPoint)
    {
        role = newRole;
        waitingPoint = newWaitingPoint;
    }

    public void Clear()
    {
        role = WorkerRole.None;
        waitingPoint = nullWaitingPoint;
    }

}
