using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour, IListener
{
    [SerializeField] Image foodimg;
    [SerializeField] TextMeshProUGUI foodName;
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
        foodName.text = data.name;
        unlockPrice.text = $"{data.unlockGoldCost}";
    }
    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventType.OnChangeRecipe, this);
    }
}
