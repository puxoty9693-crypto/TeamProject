using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CarriageSlotUI : MonoBehaviour
{
    [Header("이미지")]
    [SerializeField] Image ingredientImage;

    [Header("텍스트")]
    [SerializeField] TextMeshProUGUI costTitleText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI statText;

    [Header("버튼")]
    [SerializeField] Button actionButton;
    [SerializeField] TextMeshProUGUI actionButtonLabel;

    private Action onActionClicked;

    private void Awake()
    {
        actionButton.onClick.AddListener(() => onActionClicked?.Invoke());
    }
    public void ShowLocked(IngredientData data, int unlockCost, Action onUnlock)
    {
        ingredientImage.sprite = data.ingredientImage;
        ingredientImage.color = new Color(0.5f, 0.5f, 0.5f, 1f); // 어둡게 표시
        nameText.text = data.ingredientName;
        levelText.text = "미해금";
        statText.text = "";
        costText.text = $"{GoldFormatter.Format(unlockCost)}G";

        if (actionButtonLabel != null)
            actionButtonLabel.text = "해금";

        onActionClicked = onUnlock;
        actionButton.interactable = true;
    }
    public void ShowUnlocked(IngredientData data, int level, int supplyAmount, float supplyInterval, int upgradeCost, bool isMaxLevel, Action onUpgrade)
    {
        ingredientImage.sprite = data.ingredientImage;
        ingredientImage.color = Color.white;

        nameText.text = data.ingredientName;
        levelText.text = $"Lv.{level}";
        statText.text = $"{supplyInterval:0}초마다 {supplyAmount}개";

        if (actionButtonLabel != null)
            actionButtonLabel.text = "강화";

        onActionClicked = onUpgrade;

        if (isMaxLevel)
        {
            levelText.text = "MAX";
            costTitleText.text = "";
            costText.text = "";
            actionButton.image.color = Color.white.WithAlpha(0f);
            actionButtonLabel.text = "";
            actionButton.interactable = false;
        }

        else
        {
            costText.text = $"{GoldFormatter.Format(upgradeCost)}G";
            actionButton.interactable = true;
        }

    }
}


