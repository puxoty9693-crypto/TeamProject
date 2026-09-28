using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffSlotUI : MonoBehaviour
{
    [SerializeField] QuestRewardType rewardType;
    [SerializeField] GameObject root;
    [SerializeField] GameObject coolBackground;
    [SerializeField] Image cooldownFill;

    private float duration;
    private float remaining;
    private bool active;

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventType.OnRewardGranted, HandleRewardGranted);
    }

    private void OnDisable()
    {
        if (EventManager.HasInstance)
            EventManager.Instance.RemoveListener(EventType.OnRewardGranted, HandleRewardGranted);
    }
    private void Update()
    {
        if (!active) return;

        remaining -= Time.deltaTime;

        if (cooldownFill != null)
            cooldownFill.fillAmount = duration > 0f ? Mathf.Clamp01(remaining / duration) : 0f;

        if (remaining <= 0f)
        {
            active = false;
            root.SetActive(false);

            if (coolBackground != null)
                coolBackground.SetActive(false);
        }
    }
    private void HandleRewardGranted(Component sender, object param)
    {
        var data = (QuestRewardData)param;

        if (data.type != rewardType)
            return;

        duration = DataManager.Instance.suddenQuestConfig.buffDurationSeconds;
        remaining = duration;
        active = true;
        root.SetActive(true);
        
        if (coolBackground != null)
            coolBackground.SetActive(true);

        if (cooldownFill != null)
            cooldownFill.fillAmount = 1f;

        EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, FormatMessage(data.type, data.value));
    }

    private string FormatMessage(QuestRewardType type, float value)
    {
        return type switch
        {
            QuestRewardType.IncomeBonus => $"수익 +{value:0}% 버프 획득!",
            QuestRewardType.IngredientSupplyBuff => $"재료 수급 x{value:0.0}% 버프 획득!",
            QuestRewardType.FoodProductionBUff => $"음식 생산 +{value:0}% 버프 획득!",
            _ => "버프를 획득했습니다!"
        };
    }
}
