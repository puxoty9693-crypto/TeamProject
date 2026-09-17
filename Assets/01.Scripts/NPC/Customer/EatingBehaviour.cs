using UnityEngine;

public class EatingBehaviour : CustomerBehaviour
{
    public override CustomerState State => CustomerState.Eating;
    private float eatingEndTime;
    private float eatingTime = 3f;

    // 추후 eatingTime에 변화가 생길 수 있음.
    public float EatingTime
    {
        get => eatingTime;
        set => eatingTime = Mathf.Max(0f, value);
    }

    public override void Enter()
    {
        base.Enter();

        eatingEndTime = Time.time + eatingTime;

        GameLogOnlyEditor.Log($"{CustomerAgent.name} 식사중");
    }

    public override void Tick()
    {
        if (Time.time < eatingEndTime) return;
        AI.EatingFinished();
    }
}
