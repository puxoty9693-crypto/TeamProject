using UnityEngine;

[CreateAssetMenu(fileName = "Customer_", menuName = "Data/NPC/Customer")]
public class CustomerData : NPCData
{
    [Header("Customer")]
    [SerializeField] private float basePatience = 30f;

    public float BasePatience => basePatience;

}
