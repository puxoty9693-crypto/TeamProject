using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddIngredient(string ingredientId, int amount) 
    {
        var stock = GetStock(ingredientId);

        if (stock != null)
        {
            stock.count += amount;
        }
        else 
        {
            SaveManager.Instance.CurrentData.warehouseStock.Add(new IngredientStock { ingredientId = ingredientId, count = amount });
        }
    }

    public bool UseIngredient(string ingredientId, int amount) 
    {
        var stock = GetStock(ingredientId);
        if (stock == null || stock.count < amount) 
        {
            return false;
        }

        stock.count -= amount;
        return true;
    }
    
    public int GetIngredientCount (string ingredientId)
    {
        var stock = GetStock(ingredientId);
        return stock != null ? stock.count : 0;
    }

    private IngredientStock GetStock(string ingredientId) 
    {
        return SaveManager.Instance.CurrentData.warehouseStock.Find(s => s.ingredientId == ingredientId);
    }
    
}
