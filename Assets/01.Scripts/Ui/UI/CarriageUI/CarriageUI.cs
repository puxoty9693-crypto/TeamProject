using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarriageUI : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;

    [Header("재료업글 슬롯연결")]
    [SerializeField] List<CarriageSlotUI> slots = new();
    
    private void OnEnable()
    {
        RefreshAll();

        scrollRect.horizontalNormalizedPosition = 0f;
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
        IngredientData ingredient = data.ingredient;
    
        var unlockSystem = GameManager.Instance.ingredientUnlockSystem;
    
        var upgradeSystem = GameManager.Instance.ingredientUpgradeSystem;
    
        var supplySystem = GameManager.Instance.IngredientSupplySystem;
    
        if (!unlockSystem.IsUnlocked(ingredient))
        {
            slotUI.ShowLocked(ingredient, data.unlockGoldCost, () => TryUnlock(data, slotUI));
    
            return;
        }
    
        int level = upgradeSystem.GetCurrentLevel(ingredient);
    
        int supplyAmount = upgradeSystem.GetCurrentGatherAmount(ingredient);
    
        float supplyInterval = supplySystem.GetSupplyInterval(ingredient);
    
        int upgradeCost = upgradeSystem.GetNextUpgradeCost(ingredient);
    
        bool isMaxLevel = upgradeSystem.IsMaxLevel(ingredient);
    
        slotUI.ShowUnlocked(ingredient, level, supplyAmount, supplyInterval, upgradeCost, isMaxLevel, () => TryUpgrade(data, slotUI));
    }
    private void TryUnlock(CarriageIngredientData data, CarriageSlotUI slotUI)
    {
        bool success = GameManager.Instance.ingredientUnlockSystem.Unlock(data.ingredient);
    
        if (!success)
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "해금할 수 없습니다.");
            return;
        }
    
        EventManager.Instance.PostNotification(EventType.OnChangeGold, this, SaveManager.Instance.CurrentData.Gold);
        RefreshSlot(data, slotUI);
    }
    private void TryUpgrade(CarriageIngredientData data, CarriageSlotUI slotUI)
    {
        bool success = GameManager.Instance.ingredientUpgradeSystem.Upgrade(data.ingredient);
        if (!success)
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "업그레이드할 수 없습니다.");
            return;
        }
        EventManager.Instance.PostNotification(EventType.OnChangeGold, this, SaveManager.Instance.CurrentData.Gold);
        RefreshSlot(data, slotUI);
    }

}
