using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BatchOption 
{
    public int quantity; // 제작 개수 옵션
}

[CreateAssetMenu(fileName = "CookingBatchConfig", menuName = "Data/CookingBatchConfig")]
public class CookingBatchConfig : ScriptableObject
{
    public List<BatchOption> batchOptions; //선택 가능한 제작 수량 목록
}
