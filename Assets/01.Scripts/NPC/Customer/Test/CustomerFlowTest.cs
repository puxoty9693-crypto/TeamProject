using UnityEngine;
using UnityEngine.InputSystem;

public class CustomerFlowTest : MonoBehaviour
{
    [SerializeField] private Customer customer;
    [SerializeField] private CustomerAI customerAI;
    [SerializeField] private CustomerData customerData;

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        // 입장 + 자리 선점
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            StartTestCustomer();
        }

        // 주문 완료
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            customerAI.OrderComplete();
        }

        // 음식 도착
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            customerAI.FoodReceived();
        }

        // 식사 완료
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            customerAI.EatingFinished();
        }

        // 결제 완료
        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            customerAI.PayComplete();
        }
    }

    private void StartTestCustomer()
    {
        customer.Initialize(customerData);

        // Takeout이면 자리 필요 없음
        if (customerData.Type == CustomerData.CustomerType.Takeout)
        {
            customerAI.StartCustomer();
            return;
        }

        // DineIn은 입장 전에 자리 선점
        if (!customerAI.SeatManager.TryReserveSeat(
                customer,
                out Transform seat))
        {
            Debug.Log("빈 테이블 자리가 없어서 손님 입장 취소");
            return;
        }

        customer.SetSeat(seat);

        Debug.Log(
            $"{customer.name} 입장 전 자리 선점 : {seat.name}");

        customerAI.StartCustomer();
    }
}