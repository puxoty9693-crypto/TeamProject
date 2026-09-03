using UnityEngine;
using System;


public class ServingRequest
{
    public Transform PickUpPoint { get; private set; }
    public Transform DeliveryPoint { get; private set; }
    public Transform DumpPoint { get; private set; }

    private readonly Action onPickUp;
    private readonly Action onDelivery;
    private readonly Action onCancel;

    public bool IsCancelled { get; private set; }

    public ServingRequest(Transform PickUpPoint_, Transform DeliveryPoint_, Transform DumpPoint_, Action onPickUp_ = null, Action onDelivery_ = null, Action onCancel_ = null )
    {
        PickUpPoint = PickUpPoint_;
        DeliveryPoint = DeliveryPoint_;
        onPickUp = onPickUp_;
        onDelivery = onDelivery_;
        DumpPoint = DumpPoint_;
        onCancel = onCancel_;

    }

    public void CancelHandled()
    {
        onCancel?.Invoke();
    }

    public void Cancel()
    {
        IsCancelled = true;
    }

    public void PickUp()
    {
        onPickUp?.Invoke();
    }

    public void Delivery()
    {
        onDelivery?.Invoke();
    }
}
