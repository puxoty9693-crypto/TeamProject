using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarriageUI : MonoBehaviour
{
    [Header("재료업글 슬롯연결")]
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
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    private void RefreshSlot(CarriageIngredientData data, CarriageSlotUI slotUI)
    {
#if false
        // CarriageManager 삭제됨 (매니저 구조 변경) - 우혁님이 새 해금/강화 로직으로 교체 필요
        string ingredientId = data.ingredient.ingredientId;
        bool isUnlocked = CarriageManager.Instance.IsUnlocked(ingredientId);

        if (!isUnlocked)
        {
            slotUI.ShowLocked(data.ingredient, data.unlockGoldCost, () => TryUnlock(data, slotUI));
            return;
        }

        int level = CarriageManager.Instance.GetLevel(ingredientId);
        int clampedIndex = Mathf.Min(level, data.levels.Count - 1);
        var currentLevel = data.levels[clampedIndex];
        bool isMaxLevel = level >= data.levels.Count - 1;

        slotUI.ShowUnlocked(data.ingredient, currentLevel, level, isMaxLevel, () => TryUpgrade(data, slotUI));
#endif
    }

    private void TryUnlock(CarriageIngredientData data, CarriageSlotUI slotUI)
    {
#if false
        // CarriageManager 삭제됨 - 우혁님이 새 해금 로직으로 교체 필요
        bool success = CarriageManager.Instance.UnlockedIngredient(data.ingredient.ingredientId);

        if (!success)
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "골드가 부족합니다");
            return;
        }

        EventManager.Instance.PostNotification(EventType.OnChangeGold, this, SaveManager.Instance.CurrentData.Gold);
        RefreshSlot(data, slotUI); // 해금 성공 시 바로 강화 상태 화면으로 전환됨
#endif
    }

    private void TryUpgrade(CarriageIngredientData data, CarriageSlotUI slotUI)
    {
#if false
        // CarriageManager 삭제됨 - 우혁님이 새 강화 로직으로 교체 필요
        bool success = CarriageManager.Instance.Upgrade(data.ingredient.ingredientId);

        if (!success)
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "골드가 부족합니다");
            return;
        }

        EventManager.Instance.PostNotification(EventType.OnChangeGold, this, SaveManager.Instance.CurrentData.Gold);
        RefreshSlot(data, slotUI);
#endif
    }
}