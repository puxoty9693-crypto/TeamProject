using UnityEngine;

public class ServerBehaviour : WorkerBehaviour
{
    public override WorkerRole Role => WorkerRole.Server;

    public ServerState State { get; private set; }
    public bool IsReady => IsActive && State == ServerState.Idle;

    private ServingRequest currentRequest;

    public override void Enter()
    {
        base.Enter();

        State = ServerState.Idle;
        currentRequest = null;

    }

    public bool TryServing(ServingRequest request)
    {
        if (!IsReady) return false;

        if (request == null || request.PickUpPoint == null || request.DeliveryPoint == null) return false;

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
        if (currentRequest == null) return;
        if (!currentRequest.IsCancelled) return;
        if(State == ServerState.Delivery)
        {
            State = ServerState.Cancelled;
            SetTarget(currentRequest.DumpPoint);
        }
        
    }



    public override void Exit()
    {
        currentRequest = null;
        State = ServerState.Idle;
        base.Exit();
    }

}
