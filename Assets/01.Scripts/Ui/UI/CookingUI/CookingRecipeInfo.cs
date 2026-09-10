using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingRecipeInfo : MonoBehaviour
{
    [Header ("재료와음식정보")]
    [SerializeField] Image ingredient1Image1;
    [SerializeField] Image ingredient1Image2;
    [SerializeField] Image foodImage;
    [SerializeField] TextMeshProUGUI ingredient1Count;
    [SerializeField] TextMeshProUGUI ingredient2Count;
    
    //[SerializeField] Button cookingButton; 나중에 요리하기 만드시면 이걸로 연결해주세요

    [Header("숫자업다운")]
    [SerializeField] Button plus1Btn;
    [SerializeField] Button plus5Btn;
    [SerializeField] Button plus10Btn;
    [SerializeField] Button minus1Btn;
    [SerializeField] Button minus5Btn;
    [SerializeField] Button minus10Btn;
    [SerializeField] TextMeshProUGUI countText;

    public void UpdateCookingInfo(RecipeData data)
    {
        ingredient1Image1.sprite = data.food.requiredIngredients[0].ingredient.ingredientImage;
        ingredient1Image2.sprite = data.food.requiredIngredients[1].ingredient.ingredientImage;
        foodImage.sprite = data.food.foodImage;
        ingredient1Count.text = $"{data.food.requiredIngredients[0].amount}";
        ingredient2Count.text = $"{data.food.requiredIngredients[1].amount}";
    }
}
