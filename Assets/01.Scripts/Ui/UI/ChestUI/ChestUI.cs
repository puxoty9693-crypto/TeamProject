using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour
{
    [SerializeField]List<ChestSlotUI> slots = new();
    private void OnEnable()
    {
        SetSlots();
    }
    public void SetSlots()
    {
        List<IngredientStock> stocks = SaveManager.Instance.CurrentData.warehouseStock;

        for (int i = 0; i < slots.Count; i++)
        {
            if(i < stocks.Count)
            { 
                IngredientData data = DataManager.Instance.allIngredients.Find(x => x.ingredientId == stocks[i].ingredientId);
                slots[i].UdateChestSlotUI(data, stocks[i]);
            }
            else
            {
                slots[i].UdateChestSlotUI(null, null);
            }
        }
    }
}
