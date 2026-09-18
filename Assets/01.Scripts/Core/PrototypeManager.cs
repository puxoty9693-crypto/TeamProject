using UnityEngine;
using UnityEngine.InputSystem;

/* 테스트 목록
1. 레시피 선택
2. 제작 수량 +1 / +5 / +10
3. 현재 요청 수량 출력
4. 제작 버튼
5. 전체 필요 재료 검사
6. 제작 시작
7. 1개 제작할 때 재료 차감
8. 제작 완료
9. 음식 +1
10. 남은 제작 수량 출력
11. 재료 수급 테스트*/

public class PrototypeManager : MonoBehaviour
{
    string ownMoneyText = "소지금: ";

    float testDeltaTime = 0;

    FoodData testFood = null;

    private void Awake()
    {
        SaveManager.Instance.DeleteSave();
    }
    void Start()
    {
        testFood = DataManager.Instance.allFoods[0];

        SaveManager.Instance.CurrentData.AddGold(100000);
    }

    // Update is called once per frame
    void Update()
    {
        testDeltaTime += Time.deltaTime;

        if(testDeltaTime > 5)
        {
            testDeltaTime -= 5;
            IngredientData testIngredient = DataManager.Instance.allIngredients[0];
            GameManager.Instance.ingredientBoxController.AddIngredient(testIngredient, 5);


            IngredientBox box = GameManager.Instance.ingredientBoxController.TakeBox();
            GameManager.Instance.IngredientWareHouse.ReceiveBox(box);
            Debug.Log("박스 수급 완료");
        }

        if (Keyboard.current.f5Key.IsPressed())
        {
            TestUnlockRecipe();
        }
        if (Keyboard.current.f6Key.IsPressed())
        {
            TestTakeIngredientBox();
        }
    }

    public void TestUnlockRecipe()
    {
        IngredientData testData = DataManager.Instance.allIngredients[0];

        Debug.Log(GameManager.Instance.ingredientUnlockSystem.CanUnlock(testData));
    }

    public void TestTakeIngredientBox()
    {

    }
}
