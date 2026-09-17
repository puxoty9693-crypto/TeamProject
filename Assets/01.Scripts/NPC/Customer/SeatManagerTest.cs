using System.Collections.Generic;
using UnityEngine;

public class SeatManagerTest : MonoBehaviour
{
    [SerializeField] private SeatManager seatManager;
    [SerializeField] private Customer[] testCustomers;

    private readonly List<Transform> reservedSeats = new();


    [ContextMenu("Reserve All")]
    private void ReserveAll()
    {
        reservedSeats.Clear();

        foreach (Customer customer in testCustomers)
        {
            if (seatManager.TryReserve(customer, out Transform seat))
            {
                reservedSeats.Add(seat);

                Debug.Log(
                    $"{customer.name} -> {seat.name} / " +
                    $"{seatManager.GetTableId(seat)}");
            }
            else
            {
                Debug.Log($"{customer.name} -> Seat 없음");
            }
        }
    }


    [ContextMenu("Release First")]
    private void ReleaseFirst()
    {
        if (reservedSeats.Count == 0)
            return;

        Transform seat = reservedSeats[0];

        seatManager.ReleaseSeat(seat);
        reservedSeats.RemoveAt(0);

        Debug.Log($"{seat.name} 반환");
    }
}