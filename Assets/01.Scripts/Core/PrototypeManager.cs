using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypeManager : MonoBehaviour
{
    float testDeltaTime = 0;

    FoodData testFood = null;

    private void Awake()
    {
        SaveManager.Instance.DeleteSave();
    }
    void Start()
    {
        testFood = DataManager.Instance.allFoods[0];

        SaveManager.Instance.CurrentData.AddGold(1000000);
        EventManager.Instance.PostNotification(EventType.OnChangeGold, null, SaveManager.Instance.CurrentData.Gold);
    }

    // Update is called once per frame
    void Update()
    {
        testDeltaTime += Time.deltaTime;

        if (testDeltaTime > 5)
        {
            testDeltaTime -= 5;
            IngredientData testIngredient = DataManager.Instance.allIngredients[0];
            GameManager.Instance.ingredientBoxController.AddIngredient(testIngredient, 5);


            IngredientBox box = GameManager.Instance.ingredientBoxController.TakeBox();
            GameManager.Instance.IngredientWareHouse.ReceiveBox(box);
            Debug.Log("박스 수급 완료");
        }
    }

}
