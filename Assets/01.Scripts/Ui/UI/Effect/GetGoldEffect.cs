using UnityEngine;

public class GetGoldEffect : MonoBehaviour
{
    /* 결제 처리하는곳 아래 이 코드 추가 부탁드립니다.
     * EventManager.Instance.PostNotification(EventType.OnGetGold, this, customer.transform.position);
     */

    [SerializeField] ParticleSystem goldEffect;
    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventType.OnGetGold, PlayGoldEffect);
    }
    public void PlayGoldEffect(Component sender, object param)
    {
        if(param is Vector3 pos)
            goldEffect.transform.position = pos;

        goldEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        goldEffect.Play();
    }
    private void OnDisable()
    {
        if (EventManager.HasInstance)
            EventManager.Instance.RemoveListener(EventType.OnGetGold, PlayGoldEffect);
    }
}
