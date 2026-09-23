using UnityEngine;
using UnityEngine.UI;

public class BreakTimeButton : MonoBehaviour
{
    [SerializeField] GameObject breakTimeImg;
    [SerializeField] private Image buttonImage;
    int tutochecker = 0;

    private Color normalColor = Color.gray;
    private Color breakTimeColor = Color.white;

    public void ToggleBreakTime()
    {
        var storeSystem = GameManager.Instance.StoreSystem;
        bool nextState = !storeSystem.IsBreakTime;
        storeSystem.SetBreakTime(nextState);

        string message = nextState? "브레이크타임이 시작되었습니다. (손님 입장 중단)" : "브레이크타임이 종료되었습니다. (손님 입장 시작)";
       
        EventManager.Instance.PostNotification(EventType.OnFeedbackMessage, this, message);

        UpdateUI();

        tutochecker = 1;
    }

    private void UpdateUI()
    {
        var storeSystem = GameManager.Instance.StoreSystem;
        bool isBreak = storeSystem.IsBreakTime;

        if (buttonImage != null)
            buttonImage.color = isBreak ? breakTimeColor : normalColor;
        if(breakTimeImg != null)
            breakTimeImg.SetActive(isBreak);
        if (tutochecker != 0)
            return;
       EventManager.Instance.PostNotification(EventType.OnBreakTimeoffed, this);
    }
}
