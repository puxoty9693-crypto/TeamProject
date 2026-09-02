using System.Collections.Generic;
using UnityEngine;
//∆¿¿Â¥‘ø¿Ω√∏È ºˆ¡§
public class CarriageUI : MonoBehaviour
{
    [SerializeField] List<CarriageSlotUI> slots = new();
    private void OnEnable()
    {
        RefreshAll();
    }

    private void RefreshAll()
    {
        var carriageDataList = DataManager.Instance.carriageUpgrades;

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < carriageDataList.Count)
            {
                slots[i].gameObject.SetActive(true);
                RefreshSlot(carriageDataList[i], slots[i]);
            }
            else
            {
                slots[i].gameObject.SetActive(false); // ¿Á∑· ºˆ∫∏¥Ÿ ΩΩ∑‘¿Ã ∏π¿ª ∞ÊøÏ ≥≤¥¬ ΩΩ∑‘ º˚±Ë
            }
        }
    }

    private void RefreshSlot(CarriageIngredientData data, CarriageSlotUI slotUI)
    {
        int level = CarriageManager.Instance.GetLevel(data.ingredient.ingredientId);
        int clampedIndex = Mathf.Min(level, data.levels.Count - 1);
        var currentLevel = data.levels[clampedIndex];
        bool isMaxLevel = level >= data.levels.Count - 1;

        slotUI.UpdateCarriageUI(data.ingredient, currentLevel, level, isMaxLevel, () => TryUpgrade(data, slotUI));
    }

    private void TryUpgrade(CarriageIngredientData data, CarriageSlotUI slotUI)
    {
        bool success = CarriageManager.Instance.Upgrade(data.ingredient.ingredientId);

        if (!success)
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "∞ÒµÂ∞° ∫Œ¡∑«’¥œ¥Ÿ");
            return;
        }

        EventManager.Instance.PostNotification(EventType.OnChangeGold, this, SaveManager.Instance.CurrentData.Gold);
        RefreshSlot(data, slotUI);
    }
}
