using UnityEngine;
using UnityEngine.InputSystem;

public class MoveHousingState : IState
{
    private HousingStateController controller;
    private bool justPickedUp;
    public MoveHousingState(HousingStateController controller) { this.controller = controller; }

    public void Enter()
    {
        justPickedUp = false; 
    }
    public void Exit() => controller.ClearPreview();

    public void Update()
    {
        var grid = controller.System.Grid;

        if (!grid.IsHolding)
        {
            justPickedUp = false;
            return;
        }
        if (justPickedUp)
        {
            justPickedUp = false;
            return;
        }
        Vector2Int anchorPos = controller.GetCenteredAnchor(controller.MouseGridPos, grid.HeldObjectData.size);
        controller.UpdatePlacementPreview(anchorPos, grid.HeldObjectData);

        if (Mouse.current.leftButton.wasPressedThisFrame)
            controller.System.TryDropObject(anchorPos);

        if (Mouse.current.rightButton.wasPressedThisFrame)
            controller.CancelMove();
    }

    public void NotifyJustPickedUp()
    {
        justPickedUp = true;
    }
}