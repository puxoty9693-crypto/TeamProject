using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour, IListener
{
    [SerializeField] Image foodimg;
    [SerializeField] TextMeshProUGUI foodinfo;
    [SerializeField] TextMeshProUGUI unlockPrice;
    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventType.OnChangeRecipe, this);
    }
    public void OnEvent(EventType type, Component sender, object param)
    {
        if (type == EventType.OnChangeRecipe)
        {
            UpdateRecipeUI((RecipeData)param);
        }
    }
    public void UpdateRecipeUI(RecipeData data)
    {
        foodimg.sprite = data.food.foodImage;
        unlockPrice.text = $"{data.unlockGoldCost}";
    }
    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventType.OnChangeRecipe, this);
    }
}
