// 해금 가능한 레시피 정보
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe_", menuName = "Data/Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeId;      // 레시피 고유 ID
    public int unlockGoldCost;   // 해금에 필요한 골드
    public FoodData food;        // 연결된 음식
}