using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//팀장님오시면 수정
public class CarriageSlotUI : MonoBehaviour
{
    [SerializeField] Image ingredientImage;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI statText;
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
        statText.text = "-";
        priceText.text = $"{unlockCost}G";

        if (actionButtonLabel != null) actionButtonLabel.text = "해금";
        onActionClicked = onUnlock;
        actionButton.interactable = true;
    }
    public void ShowUnlocked(IngredientData data, CarriageIngredientLevel currentLevel, int levelIndex, bool isMaxLevel, Action onUpgrade)
    {
        ingredientImage.sprite = data.ingredientImage;
        ingredientImage.color = Color.white;
        nameText.text = data.ingredientName;
        levelText.text = $"Lv.{levelIndex + 1}";
        statText.text = currentLevel.isInfinite ? "무한 생산" : $"{currentLevel.gatherTime}초 / 1개";

        if (actionButtonLabel != null) actionButtonLabel.text = "강화";
        onActionClicked = onUpgrade;

        if (isMaxLevel)
        {
            priceText.text = "MAX";
            actionButton.interactable = false;
        }
        else
        {
            priceText.text = $"{currentLevel.upgradeGoldCost}G";
            actionButton.interactable = true;
        }
    }
}


