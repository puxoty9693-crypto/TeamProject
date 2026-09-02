using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngrendientSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] int maxCount = 999;
    [SerializeField] Sprite nullimg;
    [SerializeField] Image ingredientImg;

    public void UdateChestSlotUI(IngredientData data, IngredientStock stock)
    {
        if (data != null)
        {
            ingredientImg.sprite = data.ingredientImage;
            if(stock.count < maxCount)
            {
            countText.text = $"{stock.count}°³";
            return;
            }
            else
            {
                countText.text = $"{maxCount}°³";
                return;
            }
        }

        ingredientImg.sprite = nullimg;
        countText.text = "";
    }
}
