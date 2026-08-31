using UnityEngine;
using System.Collections.Generic;
    

public class NPCPool : MonoBehaviour
{
    [SerializeField] private Customer customerPrefab;

    [SerializeField] private int initialPoolSize = 20;

    private readonly Queue<Customer> customerPool = new();
    private readonly HashSet<Customer> pooledCustomers = new();
}
