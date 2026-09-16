using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SeatManager : MonoBehaviour
{

    // the seats what table has
    private readonly Dictionary<Transform, string> tableIds = new();
    
    // the seat who sit
    private readonly Dictionary<Transform, Customer> reservedSeats = new();

    
    private bool SyncSeats()
    {
        TableRegistry tableRegistry = TableRegistry.TryGetInstance();
        
        if (tableRegistry == null) return false;

        HashSet<Transform> currentSeats = new();

        foreach(var table in tableRegistry.GetAllTables())
        {
            string tableId = table.Key;
            Transform tableTransform = table.Value;

            if (tableTransform == null) continue;

            SeatProvider seatProvider = tableTransform.GetComponent<SeatProvider>();

            if (seatProvider == null) continue;

            foreach(Transform seat in seatProvider.Seats)
            {
                if (seat == null) continue;
                currentSeats.Add(seat);

                reservedSeats.TryAdd(seat, null);

                tableIds[seat] = tableId;
            }
        }

        List<Transform> removeSeats = new();

        foreach(Transform seat in reservedSeats.Keys)
        {
            if(seat == null || !currentSeats.Contains(seat))

            {
                removeSeats.Add(seat);
            }


        }

        foreach(Transform seat in removeSeats)
        {
            reservedSeats.Remove(seat);
            tableIds.Remove(seat);
        }

        return true;
    }
    public bool TryReserve(Customer customer, out Transform seat)
    {
        seat = null;

        if (customer == null) return false;

        SyncSeats();

        foreach(var pair in reservedSeats)
        {
            if (pair.Value != null) continue;

            reservedSeats[pair.Key] = customer;
            seat = pair.Key;

            return true;

        }
        return false;
    }

    public void ReleaseSeat(Transform seat)
    {
        if (seat == null) return;
        if (!reservedSeats.ContainsKey(seat)) return;

        reservedSeats[seat] = null;
    }
    
    
    public string GetTableId(Transform seat)
    {
        if (seat == null) return null;
        if (!tableIds.TryGetValue(seat, out string tableId)) 
        { 
            return null;
        }

        return tableId;
    }


}
