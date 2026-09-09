using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkerUpgradeSlot : MonoBehaviour
{
    [SerializeField] Image workerImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI upgradePriceText;
    [SerializeField] TextMeshProUGUI statText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Button upgradeButton;

    private event Action OnUpgradeClicked;
    public void UpdateSlot(WorkerData workerData, WorkerUpgradeLevel currentLevel, int levelIndex, bool isMaxLevel, System.Action onUpgrade)
    {
        workerImage.sprite = workerData.workerImage;
        nameText.text = workerData.workerName;
        levelText.text = $"Lv.{levelIndex + 1}";
        statText.text = $"È¿°ú {currentLevel.upgradeValue}";

        OnUpgradeClicked = onUpgrade;
        if(isMaxLevel)
        {
            upgradePriceText.text = "Max";
            upgradeButton.interactable = false;
        }
        else
        {
            upgradePriceText.text = $"{currentLevel.upgradeGoldCost}G";
            upgradeButton.interactable = true;
        }
    }
}
