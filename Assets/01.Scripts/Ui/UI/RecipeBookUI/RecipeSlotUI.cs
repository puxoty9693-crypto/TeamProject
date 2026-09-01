using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour
{
    [SerializeField] Image foodimg;
    [SerializeField] TextMeshProUGUI foodName;
    [SerializeField] TextMeshProUGUI unlockPrice;
    [SerializeField] Button upgradeButton;
    public void UpdateRecipeUI(RecipeData data)
    {
        foodimg.sprite = data.food.foodImage;
        foodName.text = data.name;
        unlockPrice.text = $"{data.unlockGoldCost}";
    }

}
