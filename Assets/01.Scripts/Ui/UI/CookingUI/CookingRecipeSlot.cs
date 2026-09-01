using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingRecipeSlot : MonoBehaviour
{
    [SerializeField] CookingRecipeInfo cookingInfo;
    [SerializeField] TextMeshProUGUI recipeName;
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
