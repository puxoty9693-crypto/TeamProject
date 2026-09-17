using UnityEngine;



/// <summary>
/// 혹시나 계산을 별도로 하게될 때 사용할 코드. 현재는 임시 테스트
/// </summary>
public class PayingBehaviour : CustomerBehaviour
{
    public override CustomerState State => CustomerState.Paying;
    private PaymentSystem paymentSystem;
    Vector3 pos = new();
    public override void Enter()
    {
        base.Enter();

        if (paymentSystem == null) paymentSystem = GameManager.Instance.PaymentSystem;

        if (CustomerAgent.OrderedFood == null) return;

        paymentSystem.Pay(CustomerAgent.OrderedFood,pos);
        GameLogOnlyEditor.Log($"{CustomerAgent.name} 결제 완료");
        AI.PayComplete();

        //if (AI.PayingPoint == null)
        //{

        //    return;
        //}

        //SetTarget(AI.PayingPoint);
    }

    //public override void Arrived()
    //{
    //    base.Arrived();

    //    Debug.Log($"{CustomerAgent.name} 계산중");


    //}


}
