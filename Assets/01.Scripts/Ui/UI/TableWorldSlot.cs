using UnityEngine;

public class TableWorldSlot : MonoBehaviour
{
    [SerializeField] string tableId;
    [SerializeField] SpriteRenderer tableRenderer;

    private void OnEnable()
    {
        RefreshSprite();
    }

    private void OnMouseDown()
    {
        TableUpgradePopup.Instance.Show(tableId,transform);
    }
    public void RefreshSprite()
    {
        var data = DataManager.Instance.tableUpgrades.Find(t => t.tableId == tableId);
        if (data == null) return;

        int level = TableManager.Instance.GetLevel(tableId);
        int clampedIndex = Mathf.Min(level, data.levels.Count - 1);
        tableRenderer.sprite = data.levels[clampedIndex].tableSprite;
    }

}
