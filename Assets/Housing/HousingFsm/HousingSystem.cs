using UnityEngine;

public class HousingSystem : MMSingleton<HousingSystem>
{
    [SerializeField] private HousingGrid housingGrid;
    [SerializeField] private GridObjectData gridObjectData;

    public HousingGrid Grid => housingGrid;
    public GridObjectData Data => gridObjectData;

    private GridObject objectToPlace;
    public GridObject ObjectToPlace => objectToPlace;

    public void SelectObjectToPlace(string objID)
    {
        var obj = gridObjectData.objects.Find(o => o.objID == objID);
        if (obj != null) objectToPlace = obj;
    }

    public bool TryPlaceObject(Vector2Int pos)
    {
        if (objectToPlace == null)
            return false;

        if (objectToPlace.objID == ObjectIds.Table)
        {
            if (TableRegistry.Instance.GetAllTables().Count >= TableManager.Instance.GetMaxTableCount())
            {
                EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "설치 가능한 테이블 수를 초과했습니다");
                return false;
            }
        }
        else if (objectToPlace.maxCount >= 0)
        {
            if (housingGrid.GetPlacedCount(objectToPlace.objID) >= objectToPlace.maxCount)
            {
                EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "설치 가능한 개수를 초과했습니다");
                return false;
            }
        }

        housingGrid.Place(pos, objectToPlace);
        EventManager.Instance.PostNotification(EventType.OnHousingChanged, this);
        return true;
    }

    public bool TryRemoveObject(GameObject target)
    {
        housingGrid.Remove(target);
        EventManager.Instance.PostNotification(EventType.OnHousingChanged, this);
        return true;
    }

    public bool TryPickUpObject(GameObject target) => housingGrid.PickUp(target);
    public bool TryDropObject(Vector2Int pos) => housingGrid.TryDrop(pos);
    public void CancelPickUp() => housingGrid.CancelPickUp();

    public void SetGridActive(bool active) => housingGrid.gameObject.SetActive(active);
}