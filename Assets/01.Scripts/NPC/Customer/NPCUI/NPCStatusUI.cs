using UnityEngine;
using UnityEngine.UI;

public class NPCStatusUI : MonoBehaviour
{
    [SerializeField] private Image progressFill;
    [SerializeField] private Image icon;

    public void ShowIcon(Sprite sprite)
    {
        gameObject.SetActive(true);
        icon.sprite = sprite;
        icon.gameObject.SetActive(sprite != null);
        progressFill.gameObject.SetActive(false);
    }

    public void ShowProgress(Sprite sprite, float progress, Color color)
    {
        gameObject.SetActive(true);
        icon.sprite = sprite;
        icon.gameObject.SetActive(sprite != null);

        progressFill.gameObject.SetActive(true);
        progressFill.fillAmount = Mathf.Clamp01(progress);
        progressFill.color = color;

    }

    public void SetProgress(float progress, Color color)
    {
        progressFill.fillAmount = Mathf.Clamp01(progress);
        progressFill.color = color;


    }

    public void Hide() 
    { 
        gameObject.SetActive(false); 
    }


}
