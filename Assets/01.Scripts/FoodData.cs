using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientAmount 
{
    public IngredientData ingredient;
    public int amount;
}




[CreateAssetMenu(fileName ="Food_", menuName ="Data/Food")]
public class FoodData : ScriptableObject
{
    public string foodId;
    public Sprite foodImage;
    public int goldPerSale;
    public List<IngredientAmount> requiredIngredients;
    public int sellPrice;
}
