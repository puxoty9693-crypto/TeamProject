using UnityEngine;

public class PrototypeManager : MonoBehaviour
{
    FoodData testFood = null;
    void Start()
    {
        SaveManager.Instance.DeleteSave();
        testFood = DataManager.Instance.allFoods[0];
        for(int i = 0; i < 10; i++) Debug.Log(GameManager.Instance.PaymentSystem.Pay(testFood));
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(SaveManager.Instance.CurrentData.Gold);
    }
}
