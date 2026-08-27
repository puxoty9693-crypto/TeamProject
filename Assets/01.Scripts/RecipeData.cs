using UnityEngine;


[CreateAssetMenu(fileName ="Recipe_", menuName = "Data/Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeId;
    public int unlockGoldCost;
    public FoodData food;
}
