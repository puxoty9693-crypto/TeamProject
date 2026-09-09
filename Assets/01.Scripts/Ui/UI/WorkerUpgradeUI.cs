using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkerUpgradeUI : MonoBehaviour
{
    [SerializeField] List<WorkerUpgradeSlot> slots;

    private void OnEnable()
    {
        var upgradeDataList = DataManager.Instance.npcUpgradeDataList;
        for(int i = 0; i < slots.Count && i < upgradeDataList.Count; i++)
        {
            RefreshSlot(upgradeDataList[i], slots[i]);
        }
    }

    private void RefreshSlot(NPCUpgradeData data, WorkerUpgradeSlot slotUI)
    {
        NPCData npcData = DataManager.Instance.allNPCs.Find(x => x.role == data.role);
        int level = SaveManager.Instance.CurrentData.GetNPCUpgradeLevel(data.role);
        int clampedIndex = Mathf.Min(level, data.levels.Count - 1);
        bool isMaxLevel = level >= data.levels.Count - 1;
        slotUI.UpdateSlot(npcData, data.levels[clampedIndex], level, isMaxLevel, () => TryUpgrade(data, slotUI));
    }
     private void TryUpgrade(NPCUpgradeData data, WorkerUpgradeSlot slotUI)
    {
        bool success = NPCUpgradeManager.Instance.Upgrade(data.role);

        if (!success)
        {
            EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, "골드가 부족합니다");
            return;
        }

        EventManager.Instance.PostNotification(EventType.OnChangeGold, this, SaveManager.Instance.CurrentData.Gold);
        RefreshSlot(data, slotUI);
    }
}
