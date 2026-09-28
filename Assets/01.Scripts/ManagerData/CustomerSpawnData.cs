using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CustomerSpawnData", menuName = "Data/CustomerSpawn")]
public class CustomerSpawnData : ScriptableObject
{
    public float spawnInterval;       // 손님이 스폰되는 주기 (초)
    public List<CustomerData> possibleCustomers; // 스폰 가능한 손님 종류 목록

}
