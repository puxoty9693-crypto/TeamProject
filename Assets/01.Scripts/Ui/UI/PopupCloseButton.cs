using UnityEngine;
using UnityEngine.UI;
public class PopupCloseButton : MonoBehaviour
{
    [Header("ÇØ´ç¹öÆ°")]
    [SerializeField] Button closeButton;
    [Header("´ÝÈú ÆË¾÷")]
    [SerializeField] GameObject targetPopup;
    private void Awake()
    {
        closeButton.onClick.AddListener(() => PopupManager.Instance.Close(targetPopup));
    }
}
