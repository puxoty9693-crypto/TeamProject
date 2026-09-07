using System;
using System.Collections.Generic;
using UnityEngine;

#region 이벤트 목록
public enum EventType
{
    OnChangeGold, //골드UI수치용이벤트
    OnGetGold, // 골드용 이펙트
    OnCustomerCount, // 손님ui수치용이벤트
    OnCustomerMaxCount, // 손님max수치용이벤트
    OnSpeechBubble, // 말풍선이벤트
    OnFeedbackMessage, // 골드부족같은 피드백용 이벤트
    OnWarehouseChanged, // 실시간 인벤토리 반영용 이벤트
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
