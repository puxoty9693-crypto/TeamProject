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
    //[SerializeField] private Transform payingPoint;
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
        if (npcPool != null) npcPool.OnCustomerReturned += HandleCustomerReturned;
        EventManager.Instance.AddListener(EventType.OnCustomerMaxCount, OnTableCapacityChanged); // 최대 스폰수 변경이벤트
        SyncMaxActiveCustomers();
    }

    private void OnDisable()
    {
        if (npcPool != null) npcPool.OnCustomerReturned -= HandleCustomerReturned;
        if (EventManager.HasInstance)
            EventManager.Instance.RemoveListener(EventType.OnCustomerMaxCount, OnTableCapacityChanged); // 최대 스폰수 변경이벤트
    }

    private void Update()
    {
        if (!CanSpawn()) return;

        spawnTimer += Time.deltaTime;

        float interval = Mathf.Max(0.1f, spawnData.spawnInterval);

        if (spawnTimer < interval) return;

        spawnTimer = 0f;

        TrySpawnCustomer();
    }

    public bool TrySpawnCustomer()
    {
        if (!CanSpawn()) return false;

        CustomerData data = GetRandomCustomerData();

        if (data == null)
        {
            GameLogOnlyEditor.Log("CustomerData 없음", gameObject);

            return false;
        }

        // 아직 비활성 상태의 Customer
        Customer customer = npcPool.Rent();

        if (customer == null)
            return false;

        // 손님이 실제로 등장하기 전에 자리부터 선점
        if (!seatManager.TryReserve(customer, out Transform seat))
        {
            npcPool.Return(customer);

            GameLogOnlyEditor.Log(" 빈 자리 없음 / Spawn 취소",gameObject);

            return false;
        }

        customer.SetSeat(seat);

        CustomerAI ai = customer.AI;

        if (ai == null)
        {
            seatManager.ReleaseSeat(seat);
            customer.ClearSeat();

            npcPool.Return(customer);

            GameLogOnlyEditor.Log($"{customer.name} : CustomerAI 없음", customer);

            return false;
        }

        // customer data로 초기화
        customer.Initialize(data);

        // Scene reference
        ai.SetRuntimeRef(seatManager, exitPoint);

        // 활성화 전 위치 배치
        customer.transform.position = spawnPoint.position;

        // 스폰
        customer.gameObject.SetActive(true);

        // NavMeshAgent 상태 초기화
        customer.Movement.ResetMovement(spawnPoint.position);

        activeCustomers.Add(customer);
        EventManager.Instance.PostNotification(EventType.OnCustomerCount, this, activeCustomers.Count);////손님입장시 ui반영 이벤트호출
        GameManager.Instance.StoreSystem.CustomerEntered();
        //Debug.Log(
        //    $"Customer Spawn : {customer.name}" +
        //    $" / Seat : {seat.name}" +
        //    $" / Active : {activeCustomers.Count}",
        //    customer);

        ai.StartCustomer();

        return true;
    }

    private bool CanSpawn()
    {
        
        if (!acceptingCustomers) return false;

        if (npcPool == null || seatManager == null || spawnPoint == null || spawnData == null) return false;
            

        if (spawnData.possibleCustomers == null || spawnData.possibleCustomers.Count == 0) return false;

        if (activeCustomers.Count >= maxActiveCustomers) return false;

        return true;
    }

    private CustomerData GetRandomCustomerData()
    {
        if (spawnData.possibleCustomers == null || spawnData.possibleCustomers.Count == 0) return null;
        

        return spawnData.possibleCustomers[Random.Range(0, spawnData.possibleCustomers.Count)];
    }

    private void HandleCustomerReturned(Customer customer)
    {
        if (customer == null) return;

        if (!activeCustomers.Remove(customer)) return;

        EventManager.Instance.PostNotification(EventType.OnCustomerCount, this, activeCustomers.Count);//손님퇴장시 ui반영 이벤트호출
        GameManager.Instance.StoreSystem.CustomerExited();
        GameLogOnlyEditor.Log($"Customer 반환 / 활성화 : {activeCustomers.Count}",gameObject);
    }

    public void SetAcceptingCustomers(bool value)
    {
        acceptingCustomers = value;

        if (!value) spawnTimer = 0f;
    }

    public void SetMaxActiveCustomers(int value)
    {
        maxActiveCustomers = Mathf.Max(0, value);
    }
    // 최대 수까지 스폰가능하게 설정
    private void OnTableCapacityChanged(Component sender, object param)
    {
        SyncMaxActiveCustomers();
    }

    private void SyncMaxActiveCustomers()
    {
        SetMaxActiveCustomers(TableManager.Instance.GetTotalCapacity());
    }
}