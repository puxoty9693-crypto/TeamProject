using UnityEngine;

// [세이브 데이터] 완성된 요리 하나의 보유 수량 [임시]
[System.Serializable]
public class FoodStock
{
    public string foodId; // FoodData.foodId와 매칭
    public int count;
}
