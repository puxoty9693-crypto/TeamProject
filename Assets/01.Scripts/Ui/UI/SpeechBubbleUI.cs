using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpeechBubbleUI : MonoBehaviour, IListener
{
    [SerializeField] TextMeshProUGUI lineText;
    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventType.OnSpeechBubble,this);
    }

    public void OnEvent(EventType type, Component sender, object param = null)
    {
        if(type == EventType.OnSpeechBubble)
        {
            DialogueEventData data = (DialogueEventData)param;

            UpdateBubble(data.target, data.line);
        }
    }
    public void UpdateBubble(Transform target, string line)
    {
        transform.position = target.position + Vector3.up * 1.5f;
        lineText.text = line;
        gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventType.OnSpeechBubble, this);
    }
}
/* 
    이대로 값넣어서 호출시켜주세요
    EventManager.Instance.PostNotification(EventType.OnSpeechBubble,this,new DialogueEventData(target, line));
*/
public class DialogueEventData
{
    public Transform target;
    public string line;

    public DialogueEventData(Transform target, string line)
    {
        this.target = target;
        this.line = line;
    }
}