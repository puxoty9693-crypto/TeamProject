using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HousingStateController : MMSingleton<HousingStateController>
{
    [SerializeField] private HousingSystem housingSystem;
    [SerializeField] GameObject grids;
    [SerializeField] GameObject pallteUI;

    public HousingSystem System => housingSystem;

    private bool isHousingMode;
    private StateMachine stateMachine = new StateMachine();

    public Vector2Int MouseGridPos { get; private set; }
    public bool MouseInGrid { get; private set; }

    private List<GameObject> previewCells = new();
    Color cellNomal = new Color(0f, 0f, 0f, 0.8f);
    Color cellGreen = new Color(0f, 1f, 0f, 0.8f);
    Color cellRed = new Color(1f, 0f, 0f, 0.8f);

    private void Update()
    {
        if (!isHousingMode)
            return;

        MouseGridPos = GetMouseGridPosition();
        MouseInGrid = housingSystem.Grid.IsInGrid(MouseGridPos);

        if (!MouseInGrid)
        {
            ClearPreview();
            return;
        }

        stateMachine.Repeat();
    }

    private Vector2Int GetMouseGridPosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f;
        return housingSystem.Grid.WorldToGrid(mouseWorldPos);
    }

    #region Housing Mode On/Off
    public void EnterHousingMode()
    {
        if(!GameManager.Instance.StoreSystem.CanHousing)
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "아직 식당안에 손님이 존재합니다.");
            return;
        }
        isHousingMode = true;
        housingSystem.SetGridActive(true);
        pallteUI.SetActive(true);
        grids.SetActive(true);
        stateMachine.ChangeState(new NormalHousingState(this));
    }

    public void ExitHousingMode()
    {
        if (housingSystem.Grid.IsHolding) housingSystem.CancelPickUp();

        stateMachine.ChangeState(null);
        isHousingMode = false;
        pallteUI.SetActive(false);
        grids.SetActive(false);
        housingSystem.SetGridActive(false);
    }
    #endregion

    #region Mode 전환 (버튼 연결용)
    public void EnterNormalMode()
    {
        if (!isHousingMode)
            return;
        stateMachine.ChangeState(new NormalHousingState(this));
    }
    public void EnterDeleteMode()
    {
        if (!isHousingMode)
            return;
        stateMachine.ChangeState(new DeleteHousingState(this));
    }
    public void EnterMoveMode()
    {
        if (!isHousingMode)
            return;
        stateMachine.ChangeState(new MoveHousingState(this));
    }
    public void ToggleDeleteMode()
    {
        if (!isHousingMode)
            return;
        if (stateMachine.currentState is DeleteHousingState)
            EnterNormalMode();
        else
            EnterDeleteMode();
    }
    public void ToggleMoveMode()
    {
        if (!isHousingMode)
            return;
        if (stateMachine.currentState is MoveHousingState)
            EnterNormalMode();
        else 
            EnterMoveMode();
    }
    #endregion

    #region 클릭 라우팅 (PlaceableObjectClickHandler가 호출)
    public void HandleObjectClicked(GameObject target)
    {
        if (!isHousingMode)
            return;

        if (stateMachine.currentState is DeleteHousingState)
            housingSystem.TryRemoveObject(target);
        else if (stateMachine.currentState is MoveHousingState)
            housingSystem.TryPickUpObject(target);
    }

    public void CancelMove()
    {
        housingSystem.CancelPickUp();
        EnterNormalMode();
    }
    #endregion

    #region 프리뷰
    public Vector2Int GetCenteredAnchor(Vector2Int mouseGridPos, Vector2Int size)
    {
        int offsetX = Mathf.FloorToInt(size.x / 2f);
        int offsetY = Mathf.FloorToInt(size.y / 2f);
        return new Vector2Int(mouseGridPos.x - offsetX, mouseGridPos.y - offsetY);
    }

    public void UpdatePlacementPreview(Vector2Int anchorPos, GridObject obj)
    {
        ClearPreview();
        var grid = housingSystem.Grid;
        foreach (Vector2Int cellPos in grid.GetObjectCells(anchorPos, obj))
        {
            GameObject cell = grid.GetCell(cellPos);
            if (cell == null) 
                continue;

            cell.GetComponent<SpriteRenderer>().color = grid.CanPlace(anchorPos, obj) ? cellGreen : cellRed;
            previewCells.Add(cell);
        }
    }

    public void UpdateDeleteHoverPreview(Vector2Int mouseGridPos)
    {
        ClearPreview();
        var grid = housingSystem.Grid;
        GameObject target = grid.GetObjectAt(mouseGridPos);
        if (target == null)
            return;

        foreach (var cellPos in grid.GetCellsOccupiedBy(target))
        {
            GameObject cell = grid.GetCell(cellPos);
            if (cell == null)
                continue;

            cell.GetComponent<SpriteRenderer>().color = cellGreen;
            previewCells.Add(cell);
        }
    }

    public void ClearPreview()
    {
        foreach (GameObject cell in previewCells)
            cell.GetComponent<SpriteRenderer>().color = cellNomal;
        previewCells.Clear();
    }
    #endregion
}