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
    public bool CanReceiveCustomer => !IsBreakTime;

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
        IsBreakTime = value;
    }
}