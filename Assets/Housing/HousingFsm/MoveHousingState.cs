using UnityEngine;
using UnityEngine.InputSystem;

public class MoveHousingState : IState
{
    private HousingStateController controller;
    public MoveHousingState(HousingStateController controller) { this.controller = controller; }

    public void Enter() { }
    public void Exit() => controller.ClearPreview();

    public void Update()
    {
        var grid = controller.System.Grid;
        if (!grid.IsHolding)
            return;

        Vector2Int anchorPos = controller.GetCenteredAnchor(controller.MouseGridPos, grid.HeldObjectData.size);
        controller.UpdatePlacementPreview(anchorPos, grid.HeldObjectData);

        if (Mouse.current.leftButton.wasPressedThisFrame)
            controller.System.TryDropObject(anchorPos);

        if (Mouse.current.rightButton.wasPressedThisFrame)
            controller.CancelMove();
    }
}