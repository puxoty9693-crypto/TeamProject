using UnityEngine;

public class WorkerWorldSlot : MonoBehaviour
{
    [SerializeField] GameObject npcUpgradePopup;

    public void OpenPopup()
    {
        PopupManager.Instance.Open(npcUpgradePopup);
    }
}