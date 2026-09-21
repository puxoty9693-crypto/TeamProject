using System;

public enum StoreState
{
    Open,
    Break
}
public class StoreSystem
{
    public StoreState State { get; private set; }

    public event Action<bool> OnStoreStateChanged;
    public bool IsOpen => State == StoreState.Open;

    public void Open()
    {
        if (State == StoreState.Open) return;

        State = StoreState.Open;

        OnStoreStateChanged?.Invoke(IsOpen);
    }

    public void StartBreak()
    {
        if (State == StoreState.Break) return;

        State = StoreState.Break;

        OnStoreStateChanged?.Invoke(IsOpen);
    }
}