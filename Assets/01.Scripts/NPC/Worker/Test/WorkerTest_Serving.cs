using UnityEngine;
using UnityEngine.InputSystem;
public class WorkerServerTest : MonoBehaviour
{
    [Header("Worker")]
    [SerializeField] private Worker worker;
    [SerializeField] private ServerBehaviour server;

    [Header("Points")]
    [SerializeField] private Transform waitingPoint;
    [SerializeField] private Transform pickUpPoint;
    [SerializeField] private Transform deliveryPoint;
    [SerializeField] private Transform dumpPoint;

    private ServingRequest currentRequest;

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            worker.Register(
                WorkerRole.Server,
                waitingPoint);

            Debug.Log("Server 등록");
        }

        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            CreateServingRequest();
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (currentRequest == null)
                return;

            currentRequest.Cancel();

            Debug.Log("ServingRequest Cancel");
        }
    }

    private void CreateServingRequest()
    {
        currentRequest = new ServingRequest(
            pickUpPoint,
            deliveryPoint,
            dumpPoint,

            onPickUp_: () =>
            {
                Debug.Log("음식 PickUp");
            },

            onDelivery_: () =>
            {
                Debug.Log("음식 Delivery");
            },

            onCancel_: () =>
            {
                Debug.Log("취소 음식 처리 완료");
            });

        bool result = server.TryServing(currentRequest);

        Debug.Log($"Serving 요청 결과 : {result}");
    }
}