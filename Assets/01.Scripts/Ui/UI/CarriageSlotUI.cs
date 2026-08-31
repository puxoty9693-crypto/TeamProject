using TMPro;
using UnityEngine;
using UnityEngine.UI;
//∆¿¿Â¥‘ø¿Ω√∏È ºˆ¡§
public class CarriageSlotUI : MonoBehaviour
{ 
    [SerializeField] Image ingredientImage;
    [SerializeField] TextMeshProUGUI UpgradePriceText;
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI LevelText;
    [SerializeField] TextMeshProUGUI CostText;
    [SerializeField] Button upgradeButton;


    public void UpdateCarriageUI(IngredientData data)
    {
        ingredientImage.sprite = data.ingredientImage;
        //LevelText.text = $"Lv.{data.level}";
    }






}


