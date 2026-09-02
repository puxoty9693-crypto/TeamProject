using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("ÅÇ ÆÐ³Î")]
    [SerializeField] GameObject ingredientTabPanel;
    [SerializeField] GameObject foodTabPanel;

    [Header("Àç·á ÅÇ ½½·Ô")]
    [SerializeField] List<IngrendientSlot> ingredientSlots = new();
    [Header("¿ä¸® ÅÇ ½½·Ô")]
    [SerializeField] List<FoodSlot> foodSlots = new();

    private void OnEnable()
    {
        ShowIngrendientTab();
    }
    public void ShowIngrendientTab()
    {
        SetTab(ingredientTabPanel);
        RefreshIngrendientSlots();
    }
    public void ShowFoodTab()
    {
        SetTab(foodTabPanel);
        RefreshFoodSlots();
    }
    private void SetTab(GameObject target)
    {
        ingredientTabPanel.SetActive(target == ingredientTabPanel);
        foodTabPanel.SetActive(target == foodTabPanel);
    }
    private void RefreshIngrendientSlots()
    {
        IReadOnlyList<IngredientStock> stocks = SaveManager.Instance.CurrentData.WarehouseStock;

        for (int i = 0; i < ingredientSlots.Count; i++)
        {
            if (i < stocks.Count)
            {
                IngredientData data = DataManager.Instance.allIngredients.Find(x => x.ingredientId == stocks[i].ingredientId);
                ingredientSlots[i].UdateChestSlotUI(data, stocks[i]);
            }
            else
            {
                ingredientSlots[i].UdateChestSlotUI(null, null);
            }
        }
    }
   private void RefreshFoodSlots()
    {
        IReadOnlyList<FoodStock> stocks = SaveManager.Instance.CurrentData.FoodStock;

        for (int i = 0; i < foodSlots.Count; i++)
        {
            if (i < stocks.Count)
            {
                FoodData data = DataManager.Instance.allFoods.Find(x => x.foodId == stocks[i].foodId);
                foodSlots[i].UdateChestSlotUI(data, stocks[i]);
            }
            else
            {
                foodSlots[i].UdateChestSlotUI(null, null);
            }
        }
    }
}
