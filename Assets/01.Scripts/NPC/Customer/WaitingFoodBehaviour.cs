using UnityEngine;

public class WaitingFoodBehaviour : CustomerBehaviour
{
    [SerializeField] private Color fullPatienceColor;
    [SerializeField] private Color lowPatienceColor;

    public override CustomerState State => CustomerState.WaitingFood;

    public override void Enter()
    {
        base.Enter();

        if(!CustomerAgent.IsPatienceActive) CustomerAgent.BeginPatience();
        if(CustomerAgent.OrderedFood != null)
        {
            float patience = CustomerAgent.PatienceNormalized;
            AI.StatusUI.ShowProgress(CustomerAgent.OrderedFood.foodImage, patience, Color.Lerp(lowPatienceColor, fullPatienceColor, patience));
        }

        
    }

    public override void Tick()
    {
        float patience = CustomerAgent.PatienceNormalized;
        AI.StatusUI.SetProgress(patience, Color.Lerp(lowPatienceColor, fullPatienceColor, patience));
    }
    public override void Exit()
    {
        AI.StatusUI.Hide();
        base.Exit();
    }
}
