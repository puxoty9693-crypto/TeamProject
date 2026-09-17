using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MMSingleton<OrderManager>
{
    private readonly List<Customer> orderList = new();

    public bool AddOrder(Customer customer)
    {
        if (customer == null) return false;
        if (orderList.Contains(customer)) return false;

        orderList.Add(customer);
        return true;
    }

    public bool RemoveOrder(Customer customer)
    {
        if (customer == null) return false;


        return orderList.Remove(customer);
    }

    public Customer GetFirstOrder()
    {
        if (orderList.Count == 0) return null;
        ;
        return orderList[0];
    }

    //public Customer NextOrder()
    //{
            // 해당 부분은 순서 변경 로직이 있을 때
    //}

}
