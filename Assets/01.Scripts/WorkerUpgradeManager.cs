using UnityEngine;

public class WorkerUpgradeManager : MMSingleton<WorkerUpgradeManager>
{
    public int GetLevel(WorkerRole role)
        => SaveManager.Instance.CurrentData.GetWorkerUpgradeLevel(role);

    public bool Upgrade(WorkerRole role)
    {
        var data = DataManager.Instance.workerUpgradeDataList.Find(w => w.role == role);
        if (data == null) return false;

        return UpgradeHelper.TryUpgrade(GetLevel(role), data.levels, () => SaveManager.Instance.CurrentData.UpgradeWorkerLevel(role));
    }
}
