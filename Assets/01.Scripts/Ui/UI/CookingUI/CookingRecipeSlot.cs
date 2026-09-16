using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingRecipeSlot : MonoBehaviour
{
    [Header("레시피info연결")]
    [SerializeField] CookingRecipeInfo cookingInfo;

    [Header("텍스트")]
    [SerializeField] TextMeshProUGUI recipeName;

    [Header("버튼")]
    [SerializeField] Button selectRecipeButton;

    RecipeData currentRecipe;
    private void Start()
    {
        selectRecipeButton.onClick.AddListener(OnClick);
    }
    public void SetRecipe(RecipeData data)
    {
        currentRecipe = data;
        recipeName.text = currentRecipe.name;
    }

    public void OnClick()
    {
        cookingInfo.UpdateCookingInfo(currentRecipe);
    }

}
