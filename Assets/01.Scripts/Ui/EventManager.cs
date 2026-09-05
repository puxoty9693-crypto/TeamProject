using System;
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
    OnWarehouseChanged,
}
#endregion
public class EventManager : MMSingleton<EventManager>
{
    private Dictionary<EventType, Action<Component, object>> events = new();

    #region 구독함수
    /*
        이벤트 구독함수
        예시) 골드
        private void OnEnable()
        {
            EventManager.Instance.AddListener(EventType.OnChangeGold,추가할 함수);
        }
        온에이블에서 구독
     */
    public void AddListener(EventType type, Action<Component, object> listener)
    {
        if (events.ContainsKey(type))
            events[type] += listener;
        else
            events[type] = listener;
    }
    #endregion

    #region 이벤트실행
    /* 이벤트 호출 함수 사용법 
     * 예시) 골드
     * EventManager.Instance.PostNotification(EventType.OnChangeGold, this, gold)  
     */
    public void PostNotification(EventType type, Component sender, object param = null)
    {
        if (events.TryGetValue(type, out var action))
            action?.Invoke(sender, param);
    }
    #endregion

    #region 구독해제
    /*
        이벤트 구독함수

        예시) 골드
        private void OnDisable()
        {
            EventManager.Instance.RemoveListener(EventType.OnChangeGold,리무브할함수);
        }
        온디스에이블에서 구독해제 *필수*
     */
    public void RemoveListener(EventType type, Action<Component, object> listener)
    {
        if (events.ContainsKey(type))
        {
            events[type] -= listener;
            if (events[type] == null)
                events.Remove(type);
        }
    }
    #endregion

}
