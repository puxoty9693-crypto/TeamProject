using UnityEngine;

public class CarriageController : MonoBehaviour
{
    private IngredientSupplySystem supplySystem;

    public void Initialize(
        IngredientSupplySystem system)
    {
        supplySystem = system;
    }

    private void Update()
    {
        if (supplySystem == null)
            return;

        supplySystem.Update(Time.deltaTime);
    }
}