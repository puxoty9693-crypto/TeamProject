using UnityEngine;
using UnityEngine.UI;

public class PopupOpenButton : MonoBehaviour
{
    [SerializeField] Button openButton;
    [SerializeField] GameObject targetPopup;
    private void Awake()
    {
        openButton.onClick.AddListener(() => PopupManager.Instance.Open(targetPopup));
    }
}
