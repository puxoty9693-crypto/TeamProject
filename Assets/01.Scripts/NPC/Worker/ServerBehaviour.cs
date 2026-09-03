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
                State = ServerState.Delivery;
                SetTarget(currentRequest.DeliveryPoint);

                break;
            case ServerState.Delivery:
                currentRequest.Delivery();
                currentRequest = null;
                State = ServerState.Idle;
                ReturnToWaitingPoint();

                break;
        }
    }

    public override void Tick()
    {

    }



    public override void Exit()
    {
        currentRequest = null;
        State = ServerState.Idle;
        base.Exit();
    }

}
