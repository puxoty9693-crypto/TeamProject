using UnityEngine;


[CreateAssetMenu(fileName = "Customer_", menuName = "Data/NPC/Customer")]
public class CustomerData : NPCData
{
    [Header("Customer")]
    [SerializeField] private float basePatience = 30f;
    [SerializeField] private CustomerType customerType = CustomerType.DineIn;

    public float BasePatience => basePatience;
    public CustomerType Type => customerType;
    

    public enum CustomerType { DineIn, Takeout } //¸Ô°í°¡´Â ¼Õ´Ô, Å×ÀÌÅ©¾Æ¿ô ¼Õ´Ô

 
}
