using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Worker))]
[RequireComponent(typeof(AgentMovement))]
public class WorkerAI : MonoBehaviour
{
    private Worker worker;
    private AgentMovement movement;

    private readonly Dictionary<WorkerRole, WorkerBehaviour> behaviours = new();

    private WorkerBehaviour currentBehaviour;
    private Transform currentTarget;
    private bool isMoving;
    private void Awake()
    {
        worker = GetComponent<Worker>();
        movement = GetComponent<AgentMovement>();

        WorkerBehaviour[] foundBehaviours = GetComponents<WorkerBehaviour>();

        foreach (WorkerBehaviour behaviour in foundBehaviours)
        {
            behaviour.Initialize(worker);

            if (behaviours.ContainsKey(behaviour.Role))
            {
                Debug.Log($"Worker Behaviour 중복 {behaviour.Role}",gameObject);

                continue;
            }

            behaviours.Add(behaviour.Role, behaviour);
        }

    }

    private void OnEnable()
    {
        worker.OnWorkerRoleChanged += HandleWorkerRoleChanged;
    }

    private void OnDisable()
    {
        worker.OnWorkerRoleChanged -= HandleWorkerRoleChanged;

        DeactivateCurrentBehaviour();
    }

    private void Start()
    {
        if (worker.RegisterWork.IsRegistered)
        {
            HandleWorkerRoleChanged(worker.RegisterWork);

        }

    }

   private void Update()
    {

        currentBehaviour?.Tick();       // worker에 업무 지시

        if (!isMoving) return;          // 이동중이 아니면 return
        if (!movement.HasArrived()) return;     // 아직 도착 안함 return

        movement.Stop();        // 이동 중지
        isMoving = false;       // 도착상태로 변경

        currentBehaviour?.Arrived();        // 도착

    }


    private void HandleWorkerRoleChanged(WorkerRegisterWork register)
    {
        DeactivateCurrentBehaviour();
        if (!register.IsRegistered)
        {
            movement.Stop();
            return;
        }

        if (!behaviours.TryGetValue(register.Role, out WorkerBehaviour behaviour))
        {
            Debug.Log($"Behaviour is Null : {register.Role}",gameObject);

            return;
        }

        currentBehaviour = behaviour;

        currentBehaviour.OnTargetChanged += HandleTargetChanged;

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

        currentBehaviour.Exit();

        currentBehaviour = null;
        currentTarget = null;
        isMoving = false;


    }

}
