using UnityEngine;


[CreateAssetMenu(fileName ="Ingredient_", menuName = "Data/Ingredient")]
public class IngredientData : ScriptableObject
{
    public string ingredientId;
    public Sprite ingredientImage;
    public string ingredientName;

}
