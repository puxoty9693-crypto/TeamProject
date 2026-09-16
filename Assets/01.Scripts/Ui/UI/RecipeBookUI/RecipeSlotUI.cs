using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour
{
    [Header( "해금재료")]
    [SerializeField] Image igredent1Img;
    [SerializeField] GameObject igredent1Dim;
    [SerializeField] Image igredent2Img;
    [SerializeField] GameObject igredent2Dim;

    [Header("레시피설명")]
    [SerializeField] Image foodimg;
    [SerializeField] TextMeshProUGUI foodName;
    [SerializeField] TextMeshProUGUI sellPrice;
    [SerializeField] TextMeshProUGUI unlockCost;
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
        sellPrice.text = $"개당 : {data.food.sellPrice}G";
        onUnlockClicked = onUnlock;

        var required = data.food.requiredIngredients;

        igredent1Img.sprite = required[0].ingredient.ingredientImage;
        bool ingredient1Unlocked = SaveManager.Instance.CurrentData.IsIngredientUnlocked(required[0].ingredient.ingredientId);
        igredent1Dim.SetActive(!ingredient1Unlocked); // 해금 안 됐으면 딤 켜기

        igredent2Img.sprite = required[1].ingredient.ingredientImage;
        bool ingredient2Unlocked = SaveManager.Instance.CurrentData.IsIngredientUnlocked(required[1].ingredient.ingredientId);
        igredent2Dim.SetActive(!ingredient2Unlocked);

        bool ingredientsReady = ingredient1Unlocked && ingredient2Unlocked;

        if (isUnlocked)
        {
            unlockCost.text = "해금 완료";
            unlockButton.interactable = false;
        }
        else if (!ingredientsReady)
        {
            unlockCost.text = "재료 미해금";
            unlockButton.interactable = false;
        }
        else
        {
            unlockCost.text = $"{GoldFormatter.Format(data.unlockGoldCost)}G";
            unlockButton.interactable = true;
        }
    }

}
