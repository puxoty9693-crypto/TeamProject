using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdUpgradeUI : MonoBehaviour
{
    [Header("텍스트")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI costTitleText;
    [SerializeField] TextMeshProUGUI costText;

    [Header("버튼")]
    [SerializeField] Button upgradeButton;
    [SerializeField] TextMeshProUGUI buttonText;

    private void Awake()
    {
        //upgradeButton.onClick.AddListener(TryUpgrade);
    }

    private void OnEnable()
    {
        //Refresh();
    }

    //private void Refresh()
    //{
    //    var levels = DataManager.Instance.adData.levels;
    //    int level = GameManager.Instance.GetLevel();
    //    bool isMaxLevel = level >= levels.Count - 1;
    //
    //    levelText.text = $"Lv.{level + 1}";
    //    descriptionText.text = GameManager.Instance.GetCurrentUpgradeText();
    //
    //    if (isMaxLevel)
    //    {
    //        levelText.text = "MAX";
    //        costTitleText.text = "";
    //        costText.text = "";
    //        upgradeButton.interactable = false;
    //    }
    //    else
    //    {
    //        costText.text = $"{GoldFormatter.Format(levels[level].upgradeGoldCost)}G";
    //        upgradeButton.interactable = true;
    //    }
    //}

    //private void TryUpgrade()
    //{
    //    bool success = GameManager.Instance.Upgrade();
    //
    //    if (!success)
    //    {
    //        EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "골드가 부족합니다");
    //        return;
    //    }
    //
    //    EventManager.Instance.PostNotification(EventType.OnChangeGold, this, SaveManager.Instance.CurrentData.Gold);
    //    Refresh();
    //}
}