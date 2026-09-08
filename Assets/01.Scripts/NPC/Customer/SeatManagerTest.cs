using UnityEngine;
using UnityEngine.InputSystem;

public class SeatManagerTest : MonoBehaviour
{
    [SerializeField] private TestSeatManager seatManager;
    [SerializeField] private Customer[] customers;

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            Reserve(0);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            Reserve(1);

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            Reserve(2);

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
            Reserve(3);

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
            Reserve(4);

        if (Keyboard.current.rKey.wasPressedThisFrame)
            Release(0);
    }

    private void Reserve(int index)
    {
        if (index < 0 || index >= customers.Length)
            return;

        Customer customer = customers[index];

        if (customer == null)
            return;

        if (customer.ReservedSeat != null)
        {
            Debug.Log(
                $"{customer.name}은 이미 {customer.ReservedSeat.name} 예약 중");
            return;
        }

        if (seatManager.TryReserveSeat(customer, out Transform seat))
        {
            customer.SetSeat(seat);

            Debug.Log(
                $"{customer.name} 좌석 예약 성공 : {seat.name}");
        }
        else
        {
            Debug.Log(
                $"{customer.name} 좌석 예약 실패 : 빈 좌석 없음");
        }
    }

    private void Release(int index)
    {
        if (index < 0 || index >= customers.Length)
            return;

        Customer customer = customers[index];

        if (customer == null)
            return;

        if (customer.ReservedSeat == null)
        {
            Debug.Log($"{customer.name}은 예약된 좌석이 없음");
            return;
        }

        Transform seat = customer.ReservedSeat;

        seatManager.ReleaseSeat(seat);
        customer.ClearSeat();

        Debug.Log(
            $"{customer.name} 좌석 해제 : {seat.name}");
    }
}