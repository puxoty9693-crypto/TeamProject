using UnityEngine;
using UnityEngine.UI;
public class PopupCloseButton : MonoBehaviour
{
    [SerializeField] Button closeButton;
    [SerializeField] GameObject targetPopup;
    private void Awake()
    {
        closeButton.onClick.AddListener(() => PopupManager.Instance.Close(targetPopup));
    }
}
