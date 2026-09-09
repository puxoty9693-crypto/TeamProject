using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
// 나중에 전부 연결해서 사용해야함

public class PopupManager : MMSingleton<PopupManager>
{
    [SerializeField] GameObject dim;
    [SerializeField] float animDuration = 0.25f;

    private List<GameObject> openPopups = new();

    public void Open(GameObject popup)
    {
        if (popup == null) return;

        if (!openPopups.Contains(popup))
            openPopups.Add(popup);

        dim.SetActive(true);
        popup.SetActive(true);

        var rect = popup.GetComponent<RectTransform>();
        rect.localScale = Vector3.zero;
        rect.DOScale(1f, animDuration).SetEase(Ease.OutBack); // 살짝 튕기며 커짐

        var cg = popup.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 0f;
            cg.DOFade(1f, animDuration);
        }
    }
    public void Close(GameObject popup)
    {
        if (popup == null)
            return;

        var rect = popup.GetComponent<RectTransform>();
        rect.DOScale(0f, animDuration).SetEase(Ease.InBack)
            .OnComplete(() => {
                popup.SetActive(false);
                openPopups.Remove(popup);
                if (openPopups.Count == 0)
                    dim.SetActive(false);
            });
    }
    public void CloseAll()
    {
        foreach (var p in openPopups)
            p.SetActive(false);

        openPopups.Clear();
        dim.SetActive(false);
    }
}
