using UnityEngine;

public class IngredientBoxController : MonoBehaviour
{
    private IngredientBox currentBox;

    public IngredientBox CurrentBox => currentBox;

    private void Awake()
    {
        currentBox = new IngredientBox();
    }

    public IngredientBox ReturnBox()
    {
        return currentBox;
    }

    // NPC가 현재 상자를 가져간다.
    public IngredientBox TakeBox()
    {
        if (currentBox == null)
            return null;

        IngredientBox takenBox = currentBox;

        // 기존 위치에는 새로운 빈 상자를 준비한다.
        currentBox = new IngredientBox();

        return takenBox;
    }

    public void AddIngredient(IngredientData ingredient, int amount)
    {
        if (currentBox == null)
            currentBox = new IngredientBox();

        currentBox.AddIngredient(ingredient, amount);
        Debug.Log(ingredient + "추가");
    }
}