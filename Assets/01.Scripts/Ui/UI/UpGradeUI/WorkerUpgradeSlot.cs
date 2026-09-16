using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkerUpgradeSlot : MonoBehaviour
{
    [Header("이름과 이미지 각자 슬롯에 따로 추가")]
     //Image workerImage;
     //TextMeshProUGUI nameText;
    [Header("텍스트")]
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI statText;
    [SerializeField] TextMeshProUGUI levelText;

    [Header("버튼")]
    [SerializeField] Button upgradeButton;

    private event Action OnUpgradeClicked;
    public void UpdateSlot( WorkerUpgradeLevel currentLevel, int levelIndex, bool isMaxLevel, System.Action onUpgrade)
    {
        levelText.text = $"Lv.{levelIndex + 1}";
        statText.text = $"효과 {currentLevel.upgradeValue}";

        OnUpgradeClicked = onUpgrade;
        if(isMaxLevel)
        {
            costText.text = "Max";
            upgradeButton.interactable = false;
        }
        else
        {
            costText.text = $"{GoldFormatter.Format(currentLevel.upgradeGoldCost)}G";
            upgradeButton.interactable = true;
        }
    }
}
