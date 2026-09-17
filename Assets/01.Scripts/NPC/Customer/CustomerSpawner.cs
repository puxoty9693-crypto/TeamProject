using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CustomerSpawnData spawnData;

    [Header("References")]
    [SerializeField] private NPCPool npcPool;
    [SerializeField] private SeatManager seatManager;

    [Header("Points")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform payingPoint;
    [SerializeField] private Transform exitPoint;

    [Header("Spawn")]
    [SerializeField] private int maxActiveCustomers = 4;

    // Store ON일 때 true.
    // OFF-Waiting / OFF에서는 false.
    [SerializeField] private bool acceptingCustomers = true;

    private float spawnTimer;

    private readonly HashSet<Customer> activeCustomers = new();

    public int ActiveCustomerCount => activeCustomers.Count;

    private void OnEnable()
    {
        if (npcPool != null)
            npcPool.OnCustomerReturned += HandleCustomerReturned;
    }

    private void OnDisable()
    {
        if (npcPool != null)
            npcPool.OnCustomerReturned -= HandleCustomerReturned;
    }

    private void Update()
    {
        if (!CanSpawn())
            return;

        spawnTimer += Time.deltaTime;

        float interval =
            Mathf.Max(0.1f, spawnData.spawnInterval);

        if (spawnTimer < interval)
            return;

        spawnTimer = 0f;

        TrySpawnCustomer();
    }

    public bool TrySpawnCustomer()
    {
        if (!CanSpawn())
            return false;

        CustomerData data = GetRandomCustomerData();

        if (data == null)
        {
            Debug.LogWarning(
                "CustomerSpawner : Spawn 가능한 CustomerData 없음",
                gameObject);

            return false;
        }

        // 아직 비활성 상태의 Customer
        Customer customer = npcPool.Rent();

        if (customer == null)
            return false;

        // 손님이 실제로 등장하기 전에 자리부터 선점
        if (!seatManager.TryReserve(
                customer,
                out Transform seat))
        {
            npcPool.Return(customer);

            Debug.Log(
                "CustomerSpawner : 빈 자리 없음 / Spawn 취소",
                gameObject);

            return false;
        }

        customer.SetSeat(seat);

        CustomerAI ai =
            customer.GetComponent<CustomerAI>();

        if (ai == null)
        {
            seatManager.ReleaseSeat(seat);
            customer.ClearSeat();

            npcPool.Return(customer);

            Debug.LogError(
                $"{customer.name} : CustomerAI 없음",
                customer);

            return false;
        }

        // 풀에서 나온 Customer에 이번 방문 Data 적용
        customer.Initialize(data);

        // Scene reference 주입
        ai.SetRuntimeRef(seatManager, payingPoint, exitPoint);

        // 활성화하기 전에 입구에 배치
        customer.transform.position = spawnPoint.position;

        // 이제 실제 등장
        customer.gameObject.SetActive(true);

        // NavMeshAgent 상태 초기화
        customer.Movement.ResetMovement(
            spawnPoint.position);

        activeCustomers.Add(customer);

        Debug.Log(
            $"Customer Spawn : {customer.name}" +
            $" / Seat : {seat.name}" +
            $" / Active : {activeCustomers.Count}",
            customer);

        ai.StartCustomer();

        return true;
    }

    private bool CanSpawn()
    {
        if (!acceptingCustomers)
            return false;

        if (npcPool == null)
            return false;

        if (seatManager == null)
            return false;

        if (spawnPoint == null)
            return false;

        if (spawnData == null)
            return false;

        if (spawnData.possibleCustomers == null ||
            spawnData.possibleCustomers.Count == 0)
        {
            return false;
        }

        if (activeCustomers.Count >= maxActiveCustomers)
            return false;

        return true;
    }

    private CustomerData GetRandomCustomerData()
    {
        if (spawnData.possibleCustomers == null ||
            spawnData.possibleCustomers.Count == 0)
        {
            return null;
        }

        return spawnData.possibleCustomers[
            Random.Range(
                0,
                spawnData.possibleCustomers.Count)];
    }

    private void HandleCustomerReturned(Customer customer)
    {
        if (customer == null)
            return;

        if (!activeCustomers.Remove(customer))
            return;

        Debug.Log(
            $"Customer Returned / Active : {activeCustomers.Count}",
            gameObject);
    }

    public void SetAcceptingCustomers(bool value)
    {
        acceptingCustomers = value;

        if (!value)
            spawnTimer = 0f;
    }

    public void SetMaxActiveCustomers(int value)
    {
        maxActiveCustomers = Mathf.Max(0, value);
    }
}