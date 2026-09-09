using UnityEngine;
using UnityEngine.UI;

public class TestGetgold : MonoBehaviour
{
    [SerializeField] Button Button;
    private void Awake()
    {
        Button.onClick.AddListener(() =>
        {
            EventManager.Instance.PostNotification(EventType.OnGetGold, this);
        });
    }

}
