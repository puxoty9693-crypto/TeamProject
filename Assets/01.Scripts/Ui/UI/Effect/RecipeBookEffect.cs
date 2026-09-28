using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBookEffect : MonoBehaviour
{
    [SerializeField] List<RectTransform> pages;
    [SerializeField] float flipDuration = 0.35f;

    private int currentIndex = 0;
    private bool isFlipping = false;

    private void OnEnable()
    {
        currentIndex = 0;
        for(int i = 0; i < pages.Count; i++)
        {
            pages[i].localRotation = Quaternion.identity;
            pages[i].gameObject.SetActive(i == currentIndex);
        }
    }
    public void NextPage()
    {
        if (isFlipping || currentIndex >= pages.Count - 1) return;
        FlipTo(currentIndex + 1, forward: true);
    }

    public void PrevPage()
    {
        if (isFlipping || currentIndex <= 0) return;
        FlipTo(currentIndex - 1, forward: false);
    }

    private void FlipTo(int newIndex, bool forward)
    {
        isFlipping = true;

        RectTransform current = pages[currentIndex];
        RectTransform next = pages[newIndex];
        float rotAngle = forward ? -90f : 90f;

        current.DOLocalRotate(new Vector3(0, rotAngle, 0), flipDuration * 0.5f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                current.gameObject.SetActive(false);
                current.localRotation = Quaternion.identity;

                next.gameObject.SetActive(true);
                next.localRotation = Quaternion.Euler(0, -rotAngle, 0);

                next.DOLocalRotate(Vector3.zero, flipDuration * 0.5f)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        currentIndex = newIndex;
                        isFlipping = false;
                    });
            });
    }
}
