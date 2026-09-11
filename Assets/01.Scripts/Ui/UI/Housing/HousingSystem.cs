using UnityEngine;
using UnityEngine.InputSystem;

public class HousingSystem : MonoBehaviour
{
    private bool isHousingMode;

    [SerializeField] private HousingGrid housingGrid;

    private void Update()
    {
        if (!isHousingMode)
            return;
        Vector3 mousWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousWorldPos.z = 0f;

        Vector2Int gridpos = housingGrid.WorldToGrid(mousWorldPos);

        Debug.Log($"현재 그리드 : {gridpos}");
    }

    public void EnterHousingMode()
    {
        isHousingMode = true;
        housingGrid.gameObject.SetActive(true);
    }
    public void ExitHousingMode()
    {
        isHousingMode = false;
        housingGrid.gameObject.SetActive(false);
    }

    public bool TryPlaceTable(Vector2Int pos, HousingGrid grid, GridObjectData data)
    {
        if (TableRegistry.Instance.GetAllTables().Count >= TableManager.Instance.GetMaxTableCount())
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "설치 가능한 테이블 수를 초과했습니다");
            return false;
        }

        var tableObj = data.objects.Find(o => o.objID == "table");
        grid.Place(pos, tableObj);
        return true;
    }

}
