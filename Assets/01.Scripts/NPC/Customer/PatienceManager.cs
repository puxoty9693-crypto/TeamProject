using UnityEngine;
using System.Collections.Generic;

public class PatienceManager : MonoBehaviour
{
    public static PatienceManager instance;

    [SerializeField] private float tickInterval = 0.2f;     // patience reduce time(s)

    private readonly HashSet<CustomerAI> customers = new();
    private readonly List<CustomerAI> tickBuffer = new();

    private float nextTickTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update() 
    {
        if (Time.time < nextTickTime) return;

        nextTickTime = Time.time + tickInterval;

        Tick();
    }

    public void Register(CustomerAI customer)
    {
        if (customer == null) return;

        customers.Add(customer);
    }

    public void Unregister(CustomerAI customer)
    {
        if (customer == null) return;
        customers.Remove(customer);
    }

    private void Tick() 
    {
        tickBuffer.Clear();
        tickBuffer.AddRange(customers);

        // CustomerAI Object인지 check
        foreach(CustomerAI customerAI in tickBuffer)
        {
            if(customerAI == null)
            {
                customers.Remove(customerAI);
                continue;
            }

            Customer customer = customerAI.Customer;
            if (!customer.IsPatienceActive) continue;

            if(customer.RemainingPatience <= 0f)
            {
                customerAI.OnPatienceExpired();
            }

        }

        

    }

    



}
