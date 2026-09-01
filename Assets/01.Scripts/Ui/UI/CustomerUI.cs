using TMPro;
using UnityEngine;

public class CustomerUI : MonoBehaviour, IListener
{
    [SerializeField] TextMeshProUGUI customerCount;
    [SerializeField] TextMeshProUGUI customerMaxCount;
    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventType.OnCustomerCount, this);
        EventManager.Instance.AddListener(EventType.OnCustomerMaxCount, this);
    }
    public void OnEvent(EventType type, Component sender, object param)
    {
        if (type == EventType.OnCustomerCount)
            UpdateCustomerCountUI((int)param);
        if (type == EventType.OnCustomerMaxCount)
            UpdateCustomerMaxCountUI((int)param);

    }
    public void UpdateCustomerCountUI(int count)
    {
        customerCount.text = $"{count}";
    }
    public void UpdateCustomerMaxCountUI(int max)
    {
        customerMaxCount.text = $"/ {max}";
    }
    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventType.OnCustomerCount, this);
        EventManager.Instance.RemoveListener(EventType.OnCustomerMaxCount, this);
    }
}
