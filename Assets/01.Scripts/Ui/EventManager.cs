using System.Collections.Generic;
using UnityEngine;

#region 이벤트 목록
public enum EventType
{
    OnChangeGold,
    OnCustomerCount,
    OnCustomerMaxCount,
    OnSpeechBubble,
    OnFeedbackMessage,

}
#endregion

#region 이벤트 구독자용 인터페이스
/*
    이벤트 구독할 cs에 사용
    예시) 골드

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
 */
public interface IListener
{
    void OnEvent(EventType type, Component sender, object param = null);
}
#endregion

public class EventManager : MMSingleton<EventManager>
{
    Dictionary<EventType, List<IListener>> listeners = new();

    #region 구독함수
    /*
        이벤트 구독함수
        예시) 골드
        private void OnEnable()
        {
            EventManager.Instance.AddListener(EventType.OnChangeGold, this);
        }
        온에이블에서 구독
     */
    public void AddListener(EventType type, IListener ilistener)
    {
        if (listeners.TryGetValue(type, out List<IListener> listenlist))
        {
            if (!listenlist.Contains(ilistener))
                listenlist.Add(ilistener);
            return;
        }

        listenlist = new List<IListener>
        {
            ilistener
        };
        listeners.Add(type, listenlist);
    }
    #endregion

    #region 이벤트실행
    /* 이벤트 호출 함수 사용법 
     * 예시) 골드
     * EventManager.Instance.PostNotification(EventType.OnChangeGold, this, gold)  
     */
    public void PostNotification(EventType type, Component sender, object param = null)
    {
        if (!listeners.TryGetValue(type, out List<IListener> listenList))
            return;
        for (int i = 0; i < listenList.Count; i++)
        {
            if (!listenList[i].Equals(null))
                listenList[i].OnEvent(type, sender, param);
        }
    }
    #endregion

    #region 구독해제
    /*
        이벤트 구독함수

        예시) 골드
        private void OnDisable()
        {
            EventManager.Instance.RemoveListener(EventType.OnChangeGold, this);
        }
        온디스에이블에서 구독해제 *필수*
     */
    public void RemoveListener(EventType type, IListener listener)
    {
        if (listeners.TryGetValue(type, out List<IListener> listenList))
        {
            listenList.Remove(listener);

            if (listenList.Count == 0)
                RemoveEvent(type);
        }
    }
    public void RemoveEvent(EventType Type)
    {
        listeners.Remove(Type);
    }
    #endregion

}
