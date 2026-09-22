public enum StoreState
{
    Empty,
    Occupied
}
public class StoreSystem
{
    public StoreState State => CustomerCount > 0 ? StoreState.Occupied : StoreState.Empty;
    public bool IsBreakTime { get; private set; }

    public int CustomerCount { get; private set; }

    public bool CanHousing => State == StoreState.Empty;
    public bool CanReceiveCustomer = false;

    public void CustomerEntered()
    {
        CustomerCount++;
    }

    public void CustomerExited()
    {
        if (CustomerCount <= 0)
            return;

        CustomerCount--;
    }

    public void SetBreakTime(bool value)
    {
        //GameLogOnlyEditor.Log($"현재 value {value}");
        IsBreakTime = value;

        //GameLogOnlyEditor.Log($"현재 isBreakTime {IsBreakTime}");
        CanReceiveCustomer = !IsBreakTime;

        //GameLogOnlyEditor.Log($"현재 CanReceiveCustomer {CanReceiveCustomer}");
    }
}