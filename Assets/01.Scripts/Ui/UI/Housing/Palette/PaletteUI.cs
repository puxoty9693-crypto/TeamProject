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
                GridObject obj = available[i];
                slots[i].gameObject.SetActive(true);

                var (current, max) = GetCountInfo(obj);
                slots[i].Setup(obj.objID, obj.icon, current, max);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    private (int current, int max) GetCountInfo(GridObject obj)
    {
        if (obj.objID == ObjectIds.Table)
        {
            int current = TableRegistry.Instance.GetAllTables().Count;
            int max = TableManager.Instance.GetMaxTableCount();
            return (current, max);
        }

        return (HousingSystem.Instance.Grid.GetPlacedCount(obj.objID), obj.maxCount);
    }

    private bool IsAvailable(GridObject obj)
    {
        if (obj.objID == ObjectIds.Table)
            return TableRegistry.Instance.GetAllTables().Count < TableManager.Instance.GetMaxTableCount();

        if (obj.maxCount < 0)
            return true;

        return HousingSystem.Instance.Grid.GetPlacedCount(obj.objID) < obj.maxCount;
    }
}