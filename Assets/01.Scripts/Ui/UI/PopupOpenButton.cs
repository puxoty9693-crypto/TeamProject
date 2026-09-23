using UnityEngine;
using UnityEngine.UI;

public class PopupOpenButton : MonoBehaviour
{
    [Header("해당버튼")]
    [SerializeField] Button openButton;
    [Header("열릴 팝업")]
    [SerializeField] GameObject targetPopup;
    private void Awake()
    {
        openButton.onClick.AddListener(() => PopupManager.Instance.Open(targetPopup));
    }
    public void TriggerOpen()
    {
        PopupManager.Instance.Open(targetPopup);
    }
}
