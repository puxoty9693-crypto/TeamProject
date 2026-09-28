using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Worker))]
[RequireComponent(typeof(AgentMovement))]
public class WorkerAI : MonoBehaviour
{
    [SerializeField] private NPCStatusUI statusUI;

    public NPCStatusUI StatusUI => statusUI;

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
            behaviour.Initialize(worker, this);

            if (behaviours.ContainsKey(behaviour.Role))
            {
                Debug.Log($"Worker Behaviour 중복 {behaviour.Role}", gameObject);
                continue;
            }

            behaviours.Add(behaviour.Role, behaviour);
        }
    }

    private void OnEnable()
    {
        worker.OnWorkerRoleChanged += HandleWorkerRoleChanged;

        if (GameManager.Instance != null && GameManager.Instance.workerUpgradeSystem != null)
            GameManager.Instance.workerUpgradeSystem.OnWorkerUpgraded += HandleWorkerUpgraded;
    }

    private void OnDisable()
    {
        worker.OnWorkerRoleChanged -= HandleWorkerRoleChanged;

        if (GameManager.Instance != null && GameManager.Instance.workerUpgradeSystem != null)
            GameManager.Instance.workerUpgradeSystem.OnWorkerUpgraded -= HandleWorkerUpgraded;

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
        currentBehaviour?.Tick();

        if (!isMoving) return;
        if (!movement.HasArrived()) return;

        movement.Stop();
        isMoving = false;

        currentBehaviour?.Arrived();
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
            Debug.Log($"Behaviour is Null : {register.Role}", gameObject);
            return;
        }

        currentBehaviour = behaviour;

        currentBehaviour.OnTargetChanged += HandleTargetChanged;

        currentBehaviour.Enter();

        RefreshSpeed(register.Role);
    }

    private void HandleTargetChanged(Transform target)
    {
        currentTarget = target;

        Debug.Log($"Target : {target?.name}");

        if (currentTarget == null)
        {
            movement.Stop();
            isMoving = false;
            return;
        }

        isMoving = movement.MoveTo(currentTarget.position);
        Debug.Log($"MovoTo : {isMoving}");
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

    private void HandleWorkerUpgraded(WorkerRole role)
    {
        if (!worker.RegisterWork.IsRegistered) return;
        if (worker.RegisterWork.Role != role) return;

        RefreshSpeed(role);
    }

    private void RefreshSpeed(WorkerRole role)
    {
        if (worker.Stats == null) return;

        float baseSpeed = movement.BaseSpeed;

        float finalSpeed = (role == WorkerRole.Server)
            ? GameManager.Instance.workerUpgradeSystem.GetUpgradedServerSpeed(baseSpeed)
            : baseSpeed;

        movement.SetSpeed(finalSpeed);
    }
}