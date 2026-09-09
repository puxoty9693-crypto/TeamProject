using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour
{
    [SerializeField] Image foodimg;
    [SerializeField] TextMeshProUGUI foodName;
    [SerializeField] TextMeshProUGUI unlockPrice;
    [SerializeField] Button unlockButton;

    private Action onUnlockClicked;
    private void Awake()
    {
        unlockButton.onClick.AddListener(() => onUnlockClicked?.Invoke());
    }

    public void UpdateRecipeUI(RecipeData data, bool isUnlocked, Action onUnlock)
    {
        foodimg.sprite = data.food.foodImage;
        foodName.text = data.food.foodId;
        onUnlockClicked = onUnlock;

        if (isUnlocked)
        {
            unlockPrice.text = "해금 완료";
            unlockButton.interactable = false;
        }
        else
        {
            unlockPrice.text = $"{data.unlockGoldCost}G";
            unlockButton.interactable = true;
        }
    }

}
