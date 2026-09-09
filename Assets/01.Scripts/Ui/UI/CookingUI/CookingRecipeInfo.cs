using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingRecipeInfo : MonoBehaviour
{
    [SerializeField] Image ingredient1Image1;
    [SerializeField] Image ingredient1Image2;
    [SerializeField] Image foodImage;
    [SerializeField] TextMeshProUGUI ingredient1Count;
    [SerializeField] TextMeshProUGUI ingredient2Count;
    //[SerializeField] Button cookingButton; 나중에 요리하기 만드시면 이걸로 연결해주세요

    public void UpdateCookingInfo(RecipeData data)
    {
        ingredient1Image1.sprite = data.food.requiredIngredients[0].ingredient.ingredientImage;
        ingredient1Image2.sprite = data.food.requiredIngredients[1].ingredient.ingredientImage;
        foodImage.sprite = data.food.foodImage;
        ingredient1Count.text = $"{data.food.requiredIngredients[0].amount}";
        ingredient2Count.text = $"{data.food.requiredIngredients[1].amount}";
    }
}
