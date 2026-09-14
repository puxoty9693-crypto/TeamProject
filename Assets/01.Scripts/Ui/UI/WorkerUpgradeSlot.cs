using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkerUpgradeSlot : MonoBehaviour
{
    [SerializeField] Image workerImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI statText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Button upgradeButton;

    private event Action OnUpgradeClicked;
    public void UpdateSlot(WokerData workerData, WorkerUpgradeLevel currentLevel, int levelIndex, bool isMaxLevel, System.Action onUpgrade)
    {
        workerImage.sprite = workerData.workerImage;
        nameText.text = workerData.workerName;
        levelText.text = $"Lv.{levelIndex + 1}";
        statText.text = $"È¿°ú {currentLevel.upgradeValue}";

        OnUpgradeClicked = onUpgrade;
        if(isMaxLevel)
        {
            costText.text = "Max";
            upgradeButton.interactable = false;
        }
        else
        {
            costText.text = $"{currentLevel.upgradeGoldCost}G";
            upgradeButton.interactable = true;
        }
    }
}
