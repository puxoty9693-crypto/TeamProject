using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableUpgradeUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI tableCountText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] Button upgradeButton;

    private void Awake()
    {
        upgradeButton.onClick.AddListener(TryUpgrade);
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void Refresh()
    {
        var levels = DataManager.Instance.tableUpgradeData.levels;
        int level = TableManager.Instance.GetLevel();
        bool isMaxLevel = level >= levels.Count - 1;

        levelText.text = $"Lv.{level + 1}";
        tableCountText.text = $"설치 가능 테이블 {TableManager.Instance.GetMaxTableCount()}개";

        if (isMaxLevel)
        {
            costText.text = "MAX";
            upgradeButton.interactable = false;
        }
        else
        {
            costText.text = $"{levels[level].upgradeGoldCost}G";
            upgradeButton.interactable = true;
        }
    }

    private void TryUpgrade()
    {
        bool success = TableManager.Instance.Upgrade();

        if (!success)
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "골드가 부족합니다");
            return;
        }

        EventManager.Instance.PostNotification(EventType.OnChangeGold, this, SaveManager.Instance.CurrentData.Gold);
        EventManager.Instance.PostNotification(EventType.OnCustomerMaxCount, this, TableManager.Instance.GetTotalCapacity());
        Refresh();
    }
}