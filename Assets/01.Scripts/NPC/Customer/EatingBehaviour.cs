using UnityEngine;

public class EatingBehaviour : CustomerBehaviour
{
    public override CustomerState State => CustomerState.Eating;

    public override void Enter()
    {
        base.Enter();

        Debug.Log($"{CustomerAgent.name} 식사중");
    }
}
