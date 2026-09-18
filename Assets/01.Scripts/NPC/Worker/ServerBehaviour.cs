using UnityEngine;

public class ServerBehaviour : WorkerBehaviour
{
    [SerializeField] private Transform pickUpPoint;
    [SerializeField] private Transform dumpPoint;
    
    
    public override WorkerRole Role => WorkerRole.Server;
    private OrderManager orderManager;

    private FoodService foodService;



    public ServerState State { get; private set; }
    public bool IsReady => IsActive && State == ServerState.Idle;

    private ServingRequest currentRequest;

    public override void Enter()
    {
        GameLogOnlyEditor.Log("Server Enter");
        base.Enter();

        TryInitializeService();

        State = ServerState.Idle;
        currentRequest = null;

    }

    public bool TryServing(ServingRequest request)
    {
        if (!IsReady) return false;

        if (request == null || request.PickUpPoint == null || request.DeliveryPoint == null||request.DumpPoint == null) return false;

        currentRequest = request;
        State = ServerState.PickUp;
        SetTarget(currentRequest.PickUpPoint);

        return true;
    }

    public override void Arrived()
    {
        base.Arrived();

        switch(State)
        {
            case ServerState.Idle:
                break;
            case ServerState.PickUp:
                currentRequest.PickUp();

                if (currentRequest.IsCancelled)
                {
                    State = ServerState.Cancelled;
                    Debug.Log($"Server State : {State}");

                    SetTarget(currentRequest.DumpPoint);
                    break;
                }

                State = ServerState.Delivery;
                Debug.Log($"Server State : {State}");

                SetTarget(currentRequest.DeliveryPoint);

                break;
            case ServerState.Delivery:
                currentRequest.Delivery();
                currentRequest = null;
                State = ServerState.Idle;
                Debug.Log($"Server State : {State}");

                ReturnToWaitingPoint();

                break;
            case ServerState.Cancelled:
                currentRequest.CancelHandled();

                currentRequest = null;
                State = ServerState.Idle;
                Debug.Log($"Server State : {State}");

                ReturnToWaitingPoint();
                break;
        }
    }

    private bool TryInitializeService()
    {
        if(orderManager == null) orderManager = OrderManager.TryGetInstance();
        if (foodService == null)
        {
            GameManager gameManager = GameManager.TryGetInstance();
            if (gameManager != null) foodService = GameManager.Instance.FoodService;
        }

        return orderManager != null && foodService != null;
    }

    public override void Tick()
    {
        GameLogOnlyEditor.Log($"Server Tick / Active:{IsActive} / Arrived:{IsArrived} / State:{State}");

        if (orderManager == null || foodService == null)
        {
            if (!TryInitializeService()) return;

            
        }


        if (currentRequest != null)
        {
            if (!currentRequest.IsCancelled) return;
            if (State == ServerState.Delivery)
            {
                State = ServerState.Cancelled;
                SetTarget(currentRequest.DumpPoint);
            }
            return;
        }

        if (!IsReady) return;

        TryNextOrder();
        
    }

    private void TryNextOrder()
    {
        GameLogOnlyEditor.Log("TrhyNextOrder 호출");
        if (orderManager == null) return;

        Customer customer = orderManager.GetFirstOrder();
        if (customer == null) return;

        if (customer.State != CustomerState.WaitingFood|| customer.OrderedFood == null || customer.ReservedSeat == null)
        {
            orderManager.RemoveOrder(customer);
            return;
        }
        GameLogOnlyEditor.Log($"주문 음식 : {customer.OrderedFood.foodName} / " + $"보유 수량 : {foodService.GetFoodCount(customer.OrderedFood)}");

        if (!foodService.HasFood(customer.OrderedFood)) return;


        ServingRequest request = new ServingRequest(pickUpPoint, customer.ReservedSeat, dumpPoint, () => foodService.TakeFood(customer.OrderedFood), () => customer.AI.FoodReceived(), 
            () =>{
            if (customer.State == CustomerState.WaitingFood) orderManager.AddOrder(customer);
        });

        if (!TryServing(request)) return;

        orderManager.RemoveOrder(customer);
    }



    public override void Exit()
    {
        currentRequest = null;
        State = ServerState.Idle;
        base.Exit();
    }

}
