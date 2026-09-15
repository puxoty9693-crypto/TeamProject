using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CustomerAI))]
[RequireComponent(typeof(Customer))]
public class CustomerMockFlow : MonoBehaviour
{
    [Header("Mock Delay")]
    [SerializeField] private float orderDelay = 0.5f;
    [SerializeField] private float foodDelay = 2f;
    [SerializeField] private float eatingDelay = 2f;
    [SerializeField] private float paymentDelay = 1f;

    private Customer customer;
    private CustomerAI ai;

    private CustomerState lastState;

    private Coroutine mockRoutine;

    private void Awake()
    {
        customer = GetComponent<Customer>();
        ai = GetComponent<CustomerAI>();
    }

    private void OnEnable()
    {
        lastState = customer.State;
    }

    private void OnDisable()
    {
        if (mockRoutine != null)
        {
            StopCoroutine(mockRoutine);
            mockRoutine = null;
        }
    }

    private void Update()
    {
        if (customer.State == lastState)
            return;

        lastState = customer.State;

        if (mockRoutine != null)
        {
            StopCoroutine(mockRoutine);
            mockRoutine = null;
        }

        switch (customer.State)
        {
            case CustomerState.Ordering:
                mockRoutine = StartCoroutine(MockOrder());
                break;

            case CustomerState.WaitingFood:
                mockRoutine = StartCoroutine(MockFood());
                break;

            case CustomerState.Eating:
                mockRoutine = StartCoroutine(MockEating());
                break;

            case CustomerState.Paying:
                mockRoutine = StartCoroutine(MockPayment());
                break;
        }
    }

    private IEnumerator MockOrder()
    {
        yield return new WaitForSeconds(orderDelay);

        if (customer.State != CustomerState.Ordering)
            yield break;

        Debug.Log($"{customer.name} ??? 주문 완료");

        ai.OrderComplete();
    }

    private IEnumerator MockFood()
    {
        yield return new WaitForSeconds(foodDelay);

        if (customer.State != CustomerState.WaitingFood)
            yield break;

        Debug.Log($"{customer.name} ??? 음식 전달");

        ai.FoodReceived();
    }

    private IEnumerator MockEating()
    {
        yield return new WaitForSeconds(eatingDelay);

        if (customer.State != CustomerState.Eating)
            yield break;

        Debug.Log($"{customer.name} ??? 식사 완료");

        ai.EatingFinished();
    }

    private IEnumerator MockPayment()
    {
        
        while (customer.State == CustomerState.Paying)
        {
            if (customer.Movement.HasArrived())
                break;

            yield return null;
        }

        if (customer.State != CustomerState.Paying)
            yield break;

        yield return new WaitForSeconds(paymentDelay);

        if (customer.State != CustomerState.Paying)
            yield break;

        Debug.Log($"{customer.name} ??? 결제 완료");

        ai.PayComplete();
    }
}