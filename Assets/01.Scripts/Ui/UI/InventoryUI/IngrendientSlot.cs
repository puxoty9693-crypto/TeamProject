using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngrendientSlot : MonoBehaviour
{
    [Header("텍스트")]
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] int maxCount = 999;
    
    [Header("이미지")]
    [SerializeField] Sprite nullimg; // null이미지
    [SerializeField] Image ingredientImg;

    public void UdateChestSlotUI(IngredientData data, IngredientStock stock)
    {
        if (data != null)
        {
            ingredientImg.sprite = data.ingredientImage;
            if(stock.count < maxCount)
            {
                countText.text = $"{stock.count}개";
                return;
            }
            else
            {
                countText.text = $"{maxCount}개";
                return;
            }
        }

        ingredientImg.sprite = nullimg;
        countText.text = "";
    }
}
