using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestSlotUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] Image ingredientImg;

    public void UdateChestSlotUI(IngredientData data, IngredientStock stock)
    {
        ingredientImg.sprite = data.ingredientImage;
        countText.text = $"{stock.count}";
    }
}
