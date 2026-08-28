// 음식 제작에 필요한 재료 + 수량 짝
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientAmount
{
    public IngredientData ingredient; // 필요한 재료
    public int amount;                // 필요한 수량
}

// 판매하는 음식 하나의 정보
[CreateAssetMenu(fileName = "Food_", menuName = "Data/Food")]
public class FoodData : ScriptableObject
{
    public string foodId;       // 음식 고유 ID
    public Sprite foodImage;    // 음식 이미지
    public int goldPerSale;     // 판매 갯수당 골드
    public List<IngredientAmount> requiredIngredients; // 필요한 재료 목록
    public int sellPrice;       // 판매 가격
}
