using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableUpgradePopup : MMSingleton<TableUpgradePopup>
{
    [SerializeField] RectTransform panel;
    [SerializeField] TextMeshProUGUI capacityText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] Button upgradeButton;
    [SerializeField] Button closeButton;
    [SerializeField] Vector3 worldOffset = new Vector3(0, 1.2f, 0);

    private string currentTableId;
    private Transform currentTarget;

    protected override void Awake()
    {
        base.Awake();
        upgradeButton.onClick.AddListener(TryUpgrade);
        closeButton.onClick.AddListener(Hide);
        panel.gameObject.SetActive(false);
    }

    public void Show(string tableId, Transform worldTarget)
    {
        currentTableId = tableId;
        currentTarget = worldTarget;
        panel.gameObject.SetActive(true);
        Refresh();
    }

    private void LateUpdate()
    {
        if (panel.gameObject.activeSelf && currentTarget != null)
        {
            panel.position = currentTarget.position + worldOffset;
        }
    }

    private void Refresh()
    {
        var data = DataManager.Instance.tableUpgrades.Find(t => t.tableId == currentTableId);
        if (data == null) return;

        int level = TableManager.Instance.GetLevel(currentTableId);
        int clampedIndex = Mathf.Min(level, data.levels.Count - 1);
        bool isMaxLevel = level >= data.levels.Count - 1;

        capacityText.text = $"수용 {data.levels[clampedIndex].capacity}명";
        costText.text = isMaxLevel ? "MAX" : $"{data.levels[level].upgradeGoldCost}G";
        upgradeButton.interactable = !isMaxLevel;
    }

    private void TryUpgrade()
    {
        bool success = TableManager.Instance.UpgradeTable(currentTableId);

        if (!success)
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "골드가 부족합니다");
            return;
        }

        EventManager.Instance.PostNotification(EventType.OnChangeGold, this, SaveManager.Instance.CurrentData.Gold);
        EventManager.Instance.PostNotification(EventType.OnCustomerMaxCount, this, TableManager.Instance.GetTotallCapacity());
        currentTarget.GetComponent<TableWorldSlot>()?.RefreshSprite();
        Refresh();
    }

    private void Hide()
    {
        panel.gameObject.SetActive(false);
        currentTarget = null;
    }
}
