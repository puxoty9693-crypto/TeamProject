using UnityEngine;

public interface IHighlightPresenter
{
    void ShowFullDim();
    void ShowHole(RectTransform target, float padding);
    void Hide();
}

public class ScreenDimHighlighter : MonoBehaviour, IHighlightPresenter
{
    [SerializeField] RectTransform dimTop;
    [SerializeField] RectTransform dimBottom;
    [SerializeField] RectTransform dimLeft;
    [SerializeField] RectTransform dimRight;
    [SerializeField] RectTransform dimCanvasRoot;

    public void Hide()
    {
        SetActive(false);
    }

    public void ShowFullDim()
    {
        SetActive(true);

        Rect full = dimCanvasRoot.rect;
        SetRect(dimTop, full.xMin, full.yMin, full.xMax, full.yMax);
        dimBottom.gameObject.SetActive(false);
        dimLeft.gameObject.SetActive(false);
        dimRight.gameObject.SetActive(false);
    }

    public void ShowHole(RectTransform target, float padding)
    {
        if (target == null)
        {
            ShowFullDim();
            return;
        }

        SetActive(true);

        Vector3[] corners = new Vector3[4];
        target.GetWorldCorners(corners);

        Canvas canvas = dimCanvasRoot.GetComponentInParent<Canvas>();
        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        Vector2 min = WorldToLocal(corners[0], cam) - Vector2.one * padding;
        Vector2 max = WorldToLocal(corners[2], cam) + Vector2.one * padding;

        Rect full = dimCanvasRoot.rect;

        SetRect(dimTop, full.xMin, max.y, full.xMax, full.yMax);
        SetRect(dimBottom, full.xMin, full.yMin, full.xMax, min.y);
        SetRect(dimLeft, full.xMin, min.y, min.x, max.y);
        SetRect(dimRight, max.x, min.y, full.xMax, max.y);
    }

    private void SetActive(bool active)
    {
        dimTop.gameObject.SetActive(active);
        dimBottom.gameObject.SetActive(active);
        dimLeft.gameObject.SetActive(active);
        dimRight.gameObject.SetActive(active);
    }

    private Vector2 WorldToLocal(Vector3 worldPoint, Camera cam)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, worldPoint);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(dimCanvasRoot, screenPoint, cam, out Vector2 localPoint);
        return localPoint;
    }

    private void SetRect(RectTransform rt, float xMin, float yMin, float xMax, float yMax)
    {
        rt.gameObject.SetActive(true);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.zero;
        rt.pivot = Vector2.zero;
        rt.anchoredPosition = new Vector2(xMin, yMin);
        rt.sizeDelta = new Vector2(Mathf.Max(0f, xMax - xMin), Mathf.Max(0f, yMax - yMin));
    }
}
