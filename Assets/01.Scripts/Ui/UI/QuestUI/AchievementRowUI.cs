using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class AchievementRowUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] Slider progressSlider;
    [SerializeField] GameObject completedCheckMark;

    public void Setup(string title, long currentValue, long threshold, bool completed) 
    {
        if (titleText != null)
            titleText.text = title;
        if (progressText != null)
            progressText.text = completed?"¿Ï·á" : $"{FormatNumber(currentValue)} / {FormatNumber(threshold)}";
        if (progressSlider != null)
            progressSlider.value = threshold > 0 ? Mathf.Clamp01((float)currentValue / threshold) : 0f;
        if (completedCheckMark != null)
            completedCheckMark.SetActive(completed);
    }

    private string FormatNumber(long value) => value.ToString("N0");
}
