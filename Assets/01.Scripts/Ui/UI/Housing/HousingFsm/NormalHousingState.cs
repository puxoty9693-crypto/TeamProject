using UnityEngine;
using UnityEngine.InputSystem;

public class NormalHousingState : IState
{
    private HousingStateController controller;
    public NormalHousingState(HousingStateController controller) { this.controller = controller; }

    public void Enter() { }
    public void Exit() => controller.ClearPreview();

    public void Update()
    {
        var grid = controller.System.Grid;
        GridObject selected = grid.IsHolding ? grid.HeldObjectData : controller.System.ObjectToPlace;

        if (selected == null) { controller.ClearPreview(); return; }

        Vector2Int anchorPos = controller.GetCenteredAnchor(controller.MouseGridPos, selected.size);
        controller.UpdatePlacementPreview(anchorPos, selected);

        if (Mouse.current.leftButton.wasPressedThisFrame)
            controller.System.TryPlaceObject(anchorPos);
    }
}