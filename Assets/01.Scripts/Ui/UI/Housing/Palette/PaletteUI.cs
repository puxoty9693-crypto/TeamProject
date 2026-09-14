using System.Collections.Generic;
using UnityEngine;

public class PaletteUI : MonoBehaviour
{
    [SerializeField] private GridObjectData gridObjectData;
    [SerializeField] private List<PaletteSlotUI> slots;

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventType.OnHousingChanged, OnHousingChanged);
        Refresh();
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventType.OnHousingChanged, OnHousingChanged);
    }

    private void OnHousingChanged(Component sender, object param) => Refresh();

    private void Refresh()
    {
        List<GridObject> available = gridObjectData.objects.FindAll(IsAvailable);

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < available.Count)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].Setup(available[i].objID, available[i].icon);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    private bool IsAvailable(GridObject obj)
    {
        if (obj.objID == ObjectIds.Table)
            return TableRegistry.Instance.GetAllTables().Count < TableManager.Instance.GetMaxTableCount();

        if (obj.maxCount < 0)
            return true; // 제한 없음

        return HousingSystem.Instance.Grid.GetPlacedCount(obj.objID) < obj.maxCount;
    }
}