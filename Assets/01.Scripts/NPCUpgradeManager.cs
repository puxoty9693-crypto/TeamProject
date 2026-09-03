using UnityEngine;

public class NPCUpgradeManager : MMSingleton<NPCUpgradeManager>
{
    public int GetLevel(NPCRole role)
        => SaveManager.Instance.CurrentData.GetNPCUpgradeLevel(role);

    public bool Upgrade(NPCRole role) 
    {
        var data = DataManager.Instance.npcUpgradeDataList.Find(n => n.role == role);
        if (data == null) return false;

       return UpgradeHelper.TryUpgrade(GetLevel(role), data.levels,() => SaveManager.Instance.CurrentData.UpgradeNPCLevel(role));
    }
}
