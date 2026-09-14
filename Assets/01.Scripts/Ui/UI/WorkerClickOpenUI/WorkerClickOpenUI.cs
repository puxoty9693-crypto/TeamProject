using UnityEngine;

public class WorkerWorldSlot : MonoBehaviour
{
    [SerializeField] GameObject workerUpgradePopup;

    public void OpenPopup()
    {
        PopupManager.Instance.Open(workerUpgradePopup);
    }
}