using UnityEngine;
using System.Collections.Generic;

public class TestSeatManager : MonoBehaviour
{
    [SerializeField] private TestSeatProvider seatProvider;

    private readonly Dictionary<Transform, Customer> occupiedSeats = new();

    private void Awake()
    {
        if (seatProvider == null) return;

        foreach (Transform seat in seatProvider.Seats)
        {
            if (seat == null) continue;

            occupiedSeats.TryAdd(seat, null);
        }


    }


    public bool TryReserveSeat(Customer customer, out Transform seat)
    {
        seat = null;
        if (customer == null) return false;

        foreach (var pair in occupiedSeats)
        {
            if (pair.Value != null) continue;

            occupiedSeats[pair.Key] = customer;
            seat = pair.Key;

            return true;
        }



        return false;
    }


    public void ReleaseSeat(Transform seat)
    {
        if (seat == null) return;

        if (!occupiedSeats.ContainsKey(seat)) return;

        occupiedSeats[seat] = null;
                
    }
}
