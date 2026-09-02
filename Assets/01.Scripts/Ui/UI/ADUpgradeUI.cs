using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdUpgradeUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI descriptionText; // 현재 레벨 효과 설명 (upgradeText)
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
        AdData data = DataManager.Instance.adData;
        int level = SaveManager.Instance.CurrentData.AdUpgradeLevel;
        int clampedIndex = Mathf.Min(level, data.levels.Count - 1);
        bool isMaxLevel = level >= data.levels.Count - 1;

        levelText.text = $"Lv.{level + 1}";
        descriptionText.text = data.levels[clampedIndex].upgradeText;

        if (isMaxLevel)
        {
            costText.text = "MAX";
            upgradeButton.interactable = false;
        }
        else
        {
            costText.text = $"{data.levels[level].upgradeGoldCost}G";
            upgradeButton.interactable = true;
        }
    }

    private void TryUpgrade()
    {
        AdData data = DataManager.Instance.adData;
        int level = SaveManager.Instance.CurrentData.AdUpgradeLevel;
        if (level >= data.levels.Count - 1) return;

        int cost = data.levels[level].upgradeGoldCost;
        if (!SaveManager.Instance.CurrentData.SpendGold(cost))
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "골드가 부족합니다");
            return;
        }

        SaveManager.Instance.CurrentData.UpgradeAdLevel();

        EventManager.Instance.PostNotification(EventType.OnChangeGold, this, SaveManager.Instance.CurrentData.Gold);
        Refresh();
    }
}