using UnityEngine;
using System.Collections.Generic;

public class SeatProvider : MonoBehaviour
{
    [SerializeField] private Transform[] seats;

    public Transform[] Seats => seats;

    private void Awake()
    {
        if (seats == null || seats.Length == 0)
        {
            seats = GetComponentsInChildren<Transform>();
        }
    }

}