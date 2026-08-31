using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour, IListener
{
    [SerializeField] TextMeshProUGUI goldText;

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventType.OnChangeGold, this);
    }

    public void OnEvent(EventType type, Component sender, object param)
    {
        if (type == EventType.OnChangeGold)
        {
            UpdateGoldUI((int)param);
        }
    }

    public void UpdateGoldUI(int gold)
    {
        goldText.text = $"{gold}G";
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventType.OnChangeGold, this);
    }

}

