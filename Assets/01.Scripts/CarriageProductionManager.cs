using System.Collections.Generic;
using UnityEngine;

public class CarriageProductionManager : MMSingleton<CarriageProductionManager>
{
    private Dictionary<string, float> productionTimers = new Dictionary<string, float>();

    private void Update()
    {
        foreach (var carriageData in DataManager.Instance.carriageUpgrades) 
        {
            string ingredientId = carriageData.ingredient.ingredientId;

            if (!CarriageManager.Instance.IsUnlocked(ingredientId)) continue;

            int level =  CarriageManager.Instance.GetLevel(ingredientId);
            if (level >= carriageData.levels.Count) continue;

            var levelData = carriageData.levels[level];

            if (!productionTimers.ContainsKey(ingredientId))
                productionTimers[ingredientId] = 0f;

            productionTimers[ingredientId] += Time.deltaTime;

            if (productionTimers[ingredientId] >= levelData.gatherTime) 
            {
                productionTimers[ingredientId] = 0f;
                SaveManager.Instance.CurrentData.AddIngredient(ingredientId, 1); // 창고에  추가
            }
        }
    }


}
