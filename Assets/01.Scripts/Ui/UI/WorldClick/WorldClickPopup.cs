using UnityEngine;

public class WorldClickPopup : MonoBehaviour, IClickable
{
    [Header("켜질 팝업연결")]
    [SerializeField] GameObject workerUpgradePopup;

    public void OnClicked()
    {
        PopupManager.Instance.Open(workerUpgradePopup);
    }
}