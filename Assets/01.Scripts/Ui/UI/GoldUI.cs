using DG.Tweening;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText;

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventType.OnChangeGold, OnChangeGold);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventType.OnChangeGold, OnChangeGold);
    }

    private void OnChangeGold(Component sender, object param)
    {
        UpdateGoldUI((int)param);
    }

    public void UpdateGoldUI(int gold)
    {
        goldText.text = $"{gold}";
    }

}

