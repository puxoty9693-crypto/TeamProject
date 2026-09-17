using DG.Tweening;
using UnityEngine;

public class ButtonPunchEffect : MonoBehaviour
{
    [SerializeField] float punchStrength = 0.15f; // Æ¨±â´Â ¼ö
    [SerializeField] float duration = 0.25f; 
    [SerializeField] int vibrato = 6; // ¶³¸²È½¼ö
    [SerializeField] float elasticity = 0.6f; // Åº¼º

    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public void OnPointerDown()
    {
        rect.DOKill();
        rect.DOPunchScale(Vector3.one * punchStrength, duration, vibrato, elasticity);
    }
    private void OnDisable()
    {
        rect.DOKill();
        rect.localScale = Vector3.one;
    }
}
