public class DeleteHousingState : IState
{
    private HousingStateController controller;
    public DeleteHousingState(HousingStateController controller) { this.controller = controller; }

    public void Enter()
    {
        if (controller.System.Grid.IsHolding) controller.System.CancelPickUp();
    }
    public void Exit() => controller.ClearPreview();
    public void Update() => controller.UpdateDeleteHoverPreview(controller.MouseGridPos);
}