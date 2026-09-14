using TMPro;
using UnityEngine;

public class CustomerUI : MonoBehaviour
{
    [Header("업데이트캔버스의 손님텍스트")]
    [SerializeField] TextMeshProUGUI customerCount;
    [SerializeField] TextMeshProUGUI customerMaxCount;
    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventType.OnCustomerCount, OnCustomerCount);
        EventManager.Instance.AddListener(EventType.OnCustomerMaxCount, OnCustomerMaxCount);
        customerMaxCount.text = $"/ {TableManager.Instance.GetTotalCapacity()}";
    }
    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventType.OnCustomerCount, OnCustomerCount);
    }
    private void OnCustomerMaxCount(Component sender, object param)
    {
        customerMaxCount.text = $"/ {(int)param}";
    }
    private void OnCustomerCount(Component sender, object param)
    {
        customerCount.text = $"{(int)param}";
    }
}
