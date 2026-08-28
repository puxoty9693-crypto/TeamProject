// 재료 하나의 기본 정보 (고정 데이터, 보유 수량은 PlayerData에서 별도 관리)
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient_", menuName = "Data/Ingredient")]
public class IngredientData : ScriptableObject
{
    public string ingredientId;   // 재료 고유 ID
    public Sprite ingredientImage; // 재료 이미지
    public string ingredientName;  // 재료 이름
}