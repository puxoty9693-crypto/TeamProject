using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] int maxCount = 999;
    [SerializeField] Sprite nullimg;
    [SerializeField] Image foodImg;

    public void UdateChestSlotUI(FoodData data, FoodStock stock)
    {
        if (data != null)
        {
            foodImg.sprite = data.foodImage;
            if (stock.count < maxCount)
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
        foodImg.sprite = nullimg;
        countText.text = "";
    }
}
