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

        SaveManager.Instance.CurrentData.AddGold(0);
        EventManager.Instance.PostNotification(EventType.OnChangeGold, null, SaveManager.Instance.CurrentData.Gold);
    }
}
