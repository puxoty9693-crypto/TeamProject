using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestSlotUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] Sprite nullimg;
    [SerializeField] int nullcount = 0;
    [SerializeField] Image ingredientImg;

    public void UdateChestSlotUI(IngredientData data, IngredientStock stock)
    {
        if (data != null)
        {
            ingredientImg.sprite = data.ingredientImage;
            countText.text = $"{stock.count}";
            return;
        }
        ingredientImg.sprite = nullimg;
        countText.text = $"{nullcount}";
    }
}
