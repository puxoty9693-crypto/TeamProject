using UnityEngine;
using System;

public class CashierBehaviour : WorkerBehaviour
{
    public override WorkerRole Role => WorkerRole.Cashier;

    public event Action<Customer> OnCustomerInteraction;
    public bool IsReady => IsActive && IsArrived;


    public override void Arrived()
    {
        base.Arrived();

        // 계산대에서 대기 애니메이션 및 UI 등
    }

    public bool TryInteract(Customer customer)
    {
        if (!IsReady) return false;
        if (customer == null) return false;

        OnCustomerInteraction?.Invoke(customer);


        return true;
    }

}
