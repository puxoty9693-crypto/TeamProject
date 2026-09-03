using UnityEngine;

<<<<<<< HEAD
[CreateAssetMenu(fileName = "Customer_", menuName = "Data/NPC/Customer")]
public class CustomerData : NPCData
{
    [Header("Customer")]
    [SerializeField] private float basePatience = 30f;

    public float BasePatience => basePatience;

=======
public enum CustomerType { DineIn, Takeout} //¸Ô°í°¡´Â ¼Õ´Ô, Å×ÀÌÅ©¾Æ¿ô ¼Õ´Ô

[CreateAssetMenu(fileName = "Customer_", menuName = "Data/Customer")]
public class CustomerData : ScriptableObject
{
    public CustomerType customerType;
    public Sprite customerImage; // ¼Õ´Ô ÀÌ¹ÌÁö
>>>>>>> Develop
}
