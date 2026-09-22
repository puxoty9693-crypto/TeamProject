using UnityEngine;

public class EatingBehaviour : CustomerBehaviour
{
    public override CustomerState State => CustomerState.Eating;
    private float eatingEndTime;
    private float eatingTime = 3f;

    [SerializeField] private Sprite eatingIcon;

    // 추후 eatingTime에 변화가 생길 수 있음.
    public float EatingTime
    {
        get => eatingTime;
        set => eatingTime = Mathf.Max(0f, value);
    }

    public override void Enter()
    {
        base.Enter();

        AI.StatusUI.ShowIcon(eatingIcon);

        eatingEndTime = Time.time + eatingTime;

        GameLogOnlyEditor.Log($"{CustomerAgent.name} 식사중");
    }

    public override void Tick()
    {
        if (Time.time < eatingEndTime) return;
        AI.EatingFinished();
    }

    public override void Exit()
    {
        AI.StatusUI.Hide();
        base.Exit();
    }
}
