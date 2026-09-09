using UnityEngine;

public class GetGoldEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem goldEffect;
    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventType.OnGetGold, PlayGoldEffect);
    }
    public void PlayGoldEffect(Component sender, object param)
    {
        goldEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        goldEffect.Play();
    }
    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventType.OnGetGold, PlayGoldEffect);
    }
}
