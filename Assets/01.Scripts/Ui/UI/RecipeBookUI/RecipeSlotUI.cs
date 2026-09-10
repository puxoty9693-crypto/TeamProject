using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour
{
    [Header( "해금재료")]
    [SerializeField] Image igredent1Img;
    [SerializeField] GameObject dimigredent1;
    [SerializeField] Image igredent2Img;
    [SerializeField] GameObject dimigredent2;

    [Header("레시피설명")]
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

        var required = data.food.requiredIngredients;

        igredent1Img.sprite = required[0].ingredient.ingredientImage;
        bool ingredient1Unlocked = CarriageManager.Instance.IsUnlocked(required[0].ingredient.ingredientId);
        dimigredent1.SetActive(!ingredient1Unlocked); // 해금 안 됐으면 딤 켜기

        igredent2Img.sprite = required[1].ingredient.ingredientImage;
        bool ingredient2Unlocked = CarriageManager.Instance.IsUnlocked(required[1].ingredient.ingredientId);
        dimigredent2.SetActive(!ingredient2Unlocked);

        bool ingredientsReady = ingredient1Unlocked && ingredient2Unlocked;

        if (isUnlocked)
        {
            unlockPrice.text = "해금 완료";
            unlockButton.interactable = false;
        }
        else if (!ingredientsReady)
        {
            unlockPrice.text = "재료 미해금";
            unlockButton.interactable = false;
        }
        else
        {
            unlockPrice.text = $"{data.unlockGoldCost}G";
            unlockButton.interactable = true;
        }
    }

}
