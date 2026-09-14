using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdUpgradeUI : MonoBehaviour
{
    [Header("텍스트")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI costText;

    [Header("버튼")]
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
        var levels = DataManager.Instance.adData.levels;
        int level = AdManager.Instance.GetLevel();
        bool isMaxLevel = level >= levels.Count - 1;

        levelText.text = $"Lv.{level + 1}";
        descriptionText.text = AdManager.Instance.GetCurrentUpgradeText();

        if (isMaxLevel)
        {
            costText.text = "MAX";
            upgradeButton.interactable = false;
        }
        else
        {
            costText.text = $"{GoldFormatter.Format(levels[level].upgradeGoldCost)}G";
            upgradeButton.interactable = true;
        }
    }

    private void TryUpgrade()
    {
        bool success = AdManager.Instance.Upgrade();

        if (!success)
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "골드가 부족합니다");
            return;
        }

        EventManager.Instance.PostNotification(EventType.OnChangeGold, this, SaveManager.Instance.CurrentData.Gold);
        Refresh();
    }
}