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

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            customer.Initialize(customerData);
            customerAI.StartCustomer();
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            customerAI.FoodReceived();
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            customerAI.EatingFinished();
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            customerAI.PayComplete();
        }
    }
}