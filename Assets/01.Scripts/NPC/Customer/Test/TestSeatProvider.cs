using UnityEngine;

public class TestSeatProvider : MonoBehaviour
{
    [SerializeField] private Transform[] seats;

    public Transform[] Seats => seats;
}
