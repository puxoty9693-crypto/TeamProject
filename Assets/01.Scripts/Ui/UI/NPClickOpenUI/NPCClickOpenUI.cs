using UnityEngine;

public class NPCWorldSlot : MonoBehaviour
{
    [SerializeField] GameObject npcUpgradePopup;

    public void OpenPopup()
    {
        PopupManager.Instance.Open(npcUpgradePopup);
    }
}