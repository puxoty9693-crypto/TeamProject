using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FeedbackUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] CanvasGroup canvasGroup; // 페이드용
    [SerializeField] float showDuration = 1.2f;
    [SerializeField] float fadeDuration = 0.3f;

    private Sequence currentSequence;

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventType.OnFeedbackMessage, OnFeedbackMessage);
        canvasGroup.alpha = 0f;
    }
    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventType.OnFeedbackMessage, OnFeedbackMessage);
        currentSequence?.Kill();
    }
    private void OnFeedbackMessage(Component sender, object param)
    {
        ShowMessage((string)param);
    }
    // param은 string으로 뭐가 부족한지 뭐가 실했는지 문구를 넣어주시면 감사하겠습니다.
    public void ShowMessage(string message)
    {
        messageText.text = message;

        currentSequence?.Kill();
        currentSequence = DOTween.Sequence()
            .Append(canvasGroup.DOFade(1f, fadeDuration))
            .AppendInterval(showDuration)
            .Append(canvasGroup.DOFade(0f, fadeDuration));
    }
}
