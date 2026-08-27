using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public List<IngredientData> allIngredients;
    public List<RecipeData> allRecipes;
    public List<FoodData> allFoods;
    public CarriageData carriageData;
    public List<NPCUpgradeData> nPCUpgradeDataList;
    public AdData adData;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
