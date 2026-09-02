using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//팀장님오시면 수정
public class CarriageSlotUI : MonoBehaviour
{
    [SerializeField] Image ingredientImage;
    [SerializeField] TextMeshProUGUI upgradePriceText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI statText;
    [SerializeField] Button upgradeButton;

    private System.Action onUpgradeClicked;
    public void UpdateCarriageUI(IngredientData data, CarriageIngredientLevel currentLevel, int levelIndex, bool isMaxLevel, Action onUpgrade)
    {
        ingredientImage.sprite = data.ingredientImage;
        nameText.text = data.ingredientName;
        levelText.text = $"Lv.{levelIndex + 1}";
        statText.text = currentLevel.isInfinite? "무한 생산" : $"{currentLevel.gatherTime}초 / {currentLevel.gatherAmount}개";

        onUpgradeClicked = onUpgrade;
        if (isMaxLevel)
        {
            upgradePriceText.text = "MAX";
            upgradeButton.interactable = false;
        }
        else
        {
            upgradePriceText.text = $"{currentLevel.upgradeGoldCost}G";
            upgradeButton.interactable = true;
        }
    }
}


