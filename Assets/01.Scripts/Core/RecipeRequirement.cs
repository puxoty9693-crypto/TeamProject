using System;
using UnityEngine;

[Serializable]
public class RecipeRequirement
{
    [SerializeField] private IngredientData ingredient;
    [SerializeField] private int amount;

    public IngredientData Ingredient => ingredient;
    public int Amount => amount;
}