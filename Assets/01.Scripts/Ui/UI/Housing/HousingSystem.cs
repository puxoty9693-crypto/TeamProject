using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum HousingMode
{
    Normal,
    Delete,
    Move,
}

public class HousingSystem : MMSingleton<HousingSystem>
{
    [SerializeField] private HousingGrid housingGrid;
    [SerializeField] private GridObjectData gridObjectData;

    private bool isHousingMode;
    public HousingMode CurrentMode { get; private set; } = HousingMode.Normal;

    private List<GameObject> previewCells = new();

    private GridObject selectedObject;

    Color cellWhite = new Color(1f, 1f, 1f, 0.8f);
    Color cellGreen = new Color(0f, 1f, 0f, 0.8f);
    Color cellRed = new Color(1f, 0f, 0f, 0.8f);

    private void Update()
    {
        if (!isHousingMode)
            return;

        Vector2Int mouseGridPos = GetMouseGridPosition();

        if (!housingGrid.IsInGrid(mouseGridPos))
        {
            ClearPreview();
            return;
        }

        switch (CurrentMode)
        {
            case HousingMode.Normal:
                UpdateNormalMode(mouseGridPos);
                break;

            case HousingMode.Delete:
                UpdateDeleteMode(mouseGridPos);
                break;

            case HousingMode.Move:
                UpdateMoveMode(mouseGridPos);
                break;
        }
    }

    #region Input
    private Vector2Int GetMouseGridPosition()
    {
        Vector3 mousWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousWorldPos.z = 0f;

        Vector2Int gridpos = housingGrid.WorldToGrid(mousWorldPos);

        return gridpos;
    }
    #endregion

    #region Housing Mode
    public void EnterHousingMode()
    {
        isHousingMode = true;
        CurrentMode = HousingMode.Normal;

        housingGrid.gameObject.SetActive(true);
    }
    public void ExitHousingMode()
    {
        if (housingGrid.IsHolding) housingGrid.CancelPickUp();

        isHousingMode = false;
        CurrentMode = HousingMode.Normal;

        housingGrid.gameObject.SetActive(false);
    }
    #endregion

    #region Mode Change
    public void EnterNormalMode()
    {
        if (!isHousingMode)
            return;

        if (housingGrid.IsHolding)
            housingGrid.CancelPickUp();

        CurrentMode = HousingMode.Normal;
    }
    public void EnterDeleteMode()
    {
        if (!isHousingMode)
            return;

        if (housingGrid.IsHolding)
            housingGrid.CancelPickUp();

        CurrentMode = HousingMode.Delete;
    }
    public void EnterMoveMode()
    {
        if (!isHousingMode)
            return;

        CurrentMode = HousingMode.Move;
    }
    public void ToggleDeleteMode()
    {
        if (!isHousingMode)
            return;

        if (CurrentMode == HousingMode.Delete)
            EnterNormalMode();
        else
            EnterDeleteMode();
    }

    public void ToggleMoveMode()
    {
        if (!isHousingMode)
            return;

        if (CurrentMode == HousingMode.Move)
            EnterNormalMode();
        else
            EnterMoveMode();
    }
    #endregion

    #region Normal Mode
    private void UpdateNormalMode(Vector2Int mouseGridPos)
    {
        selectedObject = housingGrid.IsHolding ? housingGrid.HeldObjectData : gridObjectData.objects[0];

        Vector2Int anchorPos = GetCenteredAnchor(mouseGridPos, selectedObject.size);

        UpdatePreview(anchorPos);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryPlaceTable(anchorPos);
        }
    }
    private bool TryPlaceTable(Vector2Int pos)
    {
        if (TableRegistry.Instance.GetAllTables().Count >= TableManager.Instance.GetMaxTableCount())
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "설치 가능한 테이블 수를 초과했습니다");

            return false;
        }
        var tableObj = gridObjectData.objects.Find(o => o.objID == ObjectIds.Table);
        if (tableObj == null)
            return false;

        housingGrid.Place(pos, tableObj);

        return true;
    }
    #endregion

    #region Delete Mode
    private void UpdateDeleteMode(Vector2Int mouseGridPos)
    {
        UpdateDeleteHoverPreview(mouseGridPos);
    }
    public bool TryRemove(GameObject target)
    {
        if(!isHousingMode)
            return false;

        if (CurrentMode != HousingMode.Delete)
            return false;

        housingGrid.Remove(target);
        
        return true;
    }
    #endregion

    #region Move Mode
    private void UpdateMoveMode(Vector2Int mouseGridPos)
    {
        if(!housingGrid.IsHolding)
            return;
        Vector2Int anchorPos = GetCenteredAnchor(mouseGridPos, housingGrid.HeldObjectData.size);
            
        UpdatePreview(anchorPos);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryDrop(anchorPos);
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelMove();
        }        
    }

    public bool IsHolding => housingGrid.IsHolding;

    public bool TryPickUp(GameObject target)
    {
        if (!isHousingMode)
            return false;
        if (CurrentMode != HousingMode.Move)
            return false;

        return housingGrid.PickUp(target);
    }

    public bool TryDrop(Vector2Int pos)
    {
        return housingGrid.TryDrop(pos);
    }

    public void CancelMove()
    {
        housingGrid.CancelPickUp();
        CurrentMode = HousingMode.Normal;
    }
    #endregion

    #region Preview
    private Vector2Int GetCenteredAnchor(Vector2Int mouseGridPos, Vector2Int size)
    {
        int offsetX = Mathf.FloorToInt(size.x / 2f);
        int offsetY = Mathf.FloorToInt(size.y / 2f);

        return new Vector2Int(mouseGridPos.x - offsetX, mouseGridPos.y - offsetY);
    }
    private void UpdatePreview(Vector2Int gridpos)
    {
        ClearPreview();

        List<Vector2Int> cells = housingGrid.GetObjectCells(gridpos, selectedObject);

        bool canPlace = housingGrid.CanPlace(gridpos, selectedObject);

        foreach (Vector2Int cellpos in cells)
        {
            GameObject cell = housingGrid.GetCell(cellpos);

            if (cell == null)
                continue;

            SpriteRenderer cellRenderer = cell.GetComponent<SpriteRenderer>();

            cellRenderer.color = canPlace ? cellGreen : cellRed;
            previewCells.Add(cell);
        }
    }
    private void UpdateDeleteHoverPreview(Vector2Int mouseGridPos)
    {
        ClearPreview();

        GameObject target = housingGrid.GetObjectAt(mouseGridPos);
        if (target == null)
            return;

        List<Vector2Int> cells = housingGrid.GetCellsOccupiedBy(target);
        foreach (var cellPos in cells)
        {
            GameObject cell = housingGrid.GetCell(cellPos);
            if (cell == null)
                continue;

            cell.GetComponent<SpriteRenderer>().color = cellGreen;
            previewCells.Add(cell);
        }
    }
    private void ClearPreview()
    {
        foreach (GameObject cell in previewCells)
        {
            SpriteRenderer cellRenderer = cell.GetComponent<SpriteRenderer>();
            cellRenderer.color = cellWhite;
        }
        previewCells.Clear();
    }


    #endregion

}
