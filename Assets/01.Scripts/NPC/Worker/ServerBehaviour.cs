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
        base.Enter();
        orderManager = OrderManager.TryGetInstance();
        foodService = GameManager.Instance.FoodService;
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

    public override void Tick()
    {


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
        
        if (orderManager == null) return;

        Customer customer = orderManager.GetFirstOrder();
        if (customer == null) return;

        if (customer.State != CustomerState.WaitingFood|| customer.OrderedFood == null || customer.ReservedSeat == null)
        {
            orderManager.RemoveOrder(customer);
            return;
        }
        
        
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
