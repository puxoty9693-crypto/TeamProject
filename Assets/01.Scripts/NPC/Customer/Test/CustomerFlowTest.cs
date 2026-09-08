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

            Debug.Log("Customer Flow Start");
        }
    }
}