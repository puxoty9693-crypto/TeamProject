using UnityEngine;

public class WorkerPrototypeInitializer : MonoBehaviour
{
    [Header("Workers")]
    [SerializeField] private Worker chef;
    [SerializeField] private Worker server;
    [SerializeField] private Worker cashier;

    [Header("Worker Data")]
    [SerializeField] private WorkerData chefData;
    [SerializeField] private WorkerData serverData;
    [SerializeField] private WorkerData cashierData;

    [Header("Work Points")]
    [SerializeField] private Transform chefWorkPoint;
    [SerializeField] private Transform serverWaitingPoint;
    [SerializeField] private Transform cashierWorkPoint;

    private void Start()
    {
        chef.Initialize(chefData);
        chef.Register(WorkerRole.Chef, chefWorkPoint);

        server.Initialize(serverData);
        server.Register(WorkerRole.Server, serverWaitingPoint);

        cashier.Initialize(cashierData);
        cashier.Register(WorkerRole.Cashier, cashierWorkPoint);
    }
}