using DG.Tweening.Core.Easing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class QuestPopup : MonoBehaviour
{
    [SerializeField] GameObject backgroundIMG;
    [SerializeField] GameObject lodingPanel;
    [Header("진행 패널")]
    [SerializeField] GameObject questPanel;
    [SerializeField] TextMeshProUGUI goalText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Slider progressSlider;

    [Header("결과 패널")]
    [SerializeField] GameObject resultPanel;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] float resultDisplayDuration = 2f;
    private float resultTimer;
    private bool showingResult;

    private bool questActive;
    private float remaining;
    private int goalGold;
    private QuestType currentType; //지금 뜬 퀘스트가 골드인지 음식인지

    private QuestType currentType;
    private void OnEnable()
    {
        backgroundIMG.SetActive(false);
        questPanel.SetActive(false);
        resultPanel.SetActive(false);
        lodingPanel.SetActive(false);

        EventManager.Instance.AddListener(EventType.OnQuestStarted, OnQuestStarted);
        EventManager.Instance.AddListener(EventType.OnQuestEnded, OnQuestEnded);

    }
    private void OnDisable()
    {
        if (EventManager.HasInstance)
        {
            EventManager.Instance.RemoveListener(EventType.OnQuestStarted, OnQuestStarted);
            EventManager.Instance.RemoveListener(EventType.OnQuestEnded, OnQuestEnded);
        }
    }

    private void Update()
    {
        if (questActive)
        {
            remaining -= Time.deltaTime;
            timerText.text = $"{Mathf.Max(0f, remaining):0.0}초";

            if (progressSlider != null)
            {
                int achieved = GameManager.Instance.QuestManager.ChallengeAchieved;
                progressSlider.value = goalGold > 0 ? Mathf.Clamp01((float)achieved / goalGold) : 0f;
            }
        }

        if (showingResult)
        {
            resultTimer -= Time.deltaTime;
            if (resultTimer <= 0f)
            {
                showingResult = false;
                resultPanel.SetActive(false);
                backgroundIMG.SetActive(false);
            }
        }
    }

    private void OnQuestStarted(Component sender, object param)
    {
        var data = (QuestStartedData)param;
        backgroundIMG.SetActive(true);
        questActive = true;
        remaining = data.duration;
        goalGold = data.goalGold;
        currentType = data.type;

        questPanel.SetActive(true);
        goalText.text = currentType == QuestType.EarnGoldWithTime ? $"목표: {GoldFormatter.Format(goalGold)}G" : $"목표: {goalGold}개 판매";

        if (progressSlider != null)
            progressSlider.value = 0f;

    }

    private void OnQuestEnded(Component sender, object param)
    {
        bool success = (bool)param;

        questActive = false;
        questPanel.SetActive(false);

        resultText.text = success ? "퀘스트 성공!" : "퀘스트 실패";
        resultPanel.SetActive(true);
        resultTimer = resultDisplayDuration;
        showingResult = true;
    }
}
