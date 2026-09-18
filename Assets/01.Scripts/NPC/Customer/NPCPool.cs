using System.Collections.Generic;
using UnityEngine;
using System;


public class NPCPool : MonoBehaviour
{
    [Header("Customer Prefab")]
    [SerializeField]
    private string customerPrefabFolder = "Assets/03.Prefabs/NPC/Customer";

    [SerializeField]
    private List<Customer> customerPrefabs = new();

    [Header("Pool")]
    [SerializeField] private int initialPoolSize = 20;
    [SerializeField] private Transform poolRoot;

    // prefab 이름 -> 해당 prefab 인스턴스 풀
    private readonly Dictionary<string, Queue<Customer>> customerPools = new();

    // 생성된 Customer가 어느 prefab에서 만들어졌는지 기억
    private readonly Dictionary<Customer, string> instancePoolKeys = new();

    // 이미 풀 안에 들어가 있는 Customer
    private readonly HashSet<Customer> pooledCustomers = new();

    public event Action<Customer> OnCustomerReturned;

    private void Awake()
    {
        if (poolRoot == null)
            poolRoot = transform;

        InitializePool();
    }

    private void InitializePool()
    {
        if (customerPrefabs == null || customerPrefabs.Count == 0)
        {
            Debug.LogError("NPCPool : 등록된 Customer Prefab이 없음", gameObject);
            return;
        }

        // 각 prefab용 Queue 생성
        foreach (Customer prefab in customerPrefabs)
        {
            if (prefab == null)
                continue;

            if (!customerPools.ContainsKey(prefab.name))
            {
                customerPools.Add(
                    prefab.name,
                    new Queue<Customer>());
            }
        }

        // initialPoolSize는 전체 개수
        // prefab 종류별로 순서대로 분배
        for (int i = 0; i < initialPoolSize; i++)
        {
            Customer prefab =
                customerPrefabs[i % customerPrefabs.Count];

            if (prefab == null)
                continue;

            Customer instance = CreateInstance(prefab);

            AddToPool(instance);
        }

        Debug.Log(
            $"NPCPool 초기화 완료 : {instancePoolKeys.Count}개",
            gameObject);
    }

    /// <summary>
    /// 풀에서 Customer
    /// </summary>
    public Customer Rent()
    {
        if (customerPrefabs == null ||
            customerPrefabs.Count == 0)
        {
            Debug.LogError("NPCPool : Customer Prefab 없음");
            return null;
        }

        Customer prefab =
            customerPrefabs[
                UnityEngine.Random.Range(0, customerPrefabs.Count)];

        string key = prefab.name;

        Customer customer;

        // 해당 종류의 남은 객체가 없으면 확장
        if (customerPools[key].Count == 0)
        {
            customer = CreateInstance(prefab);

            Debug.Log(
                $"NPCPool 확장 : {prefab.name}",
                gameObject);
        }
        else
        {
            customer = customerPools[key].Dequeue();
            pooledCustomers.Remove(customer);
        }

        
        // Rent
        // 자리 예약
        // 초기화
        // 위치 설정
        // 활성화
       

        return customer;
    }

    /// <summary>
    /// Customer를 풀로 반환.
    /// </summary>
    public void Return(Customer customer)
    {
        if (customer == null)
            return;

        // 이미 반환된 객체의 중복 Return 방지
        if (pooledCustomers.Contains(customer))
            return;

        if (!instancePoolKeys.ContainsKey(customer))
        {
            Debug.LogWarning(
                $"NPCPool에서 생성하지 않은 Customer 반환 시도 : {customer.name}",
                customer);

            return;
        }

        CustomerAI ai =
            customer.GetComponent<CustomerAI>();

        if (ai != null)
            ai.StopCustomer();

        customer.ResetRun();

        customer.transform.SetParent(poolRoot);
        customer.gameObject.SetActive(false);

        AddToPool(customer);

        OnCustomerReturned?.Invoke(customer);

        Debug.Log(
            $"NPCPool Return : {customer.name}",
            gameObject);
    }

    private Customer CreateInstance(Customer prefab)
    {
        Customer instance =
            Instantiate(prefab, poolRoot);

        instance.name = prefab.name;

        string key = prefab.name;

        instancePoolKeys.Add(instance, key);

        CustomerAI ai =
            instance.GetComponent<CustomerAI>();

        if (ai != null)
            ai.OnExitComplete += HandleExitComplete;

        instance.gameObject.SetActive(false);

        return instance;
    }

    private void AddToPool(Customer customer)
    {
        if (customer == null)
            return;

        if (!instancePoolKeys.TryGetValue(
                customer,
                out string key))
        {
            return;
        }

        if (pooledCustomers.Contains(customer))
            return;

        pooledCustomers.Add(customer);
        customerPools[key].Enqueue(customer);
    }

    private void HandleExitComplete(CustomerAI ai)
    {
        if (ai == null)
            return;

        Return(ai.Customer);
    }

}