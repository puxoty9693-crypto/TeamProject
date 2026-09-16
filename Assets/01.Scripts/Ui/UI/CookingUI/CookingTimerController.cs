using UnityEngine;

// 조리 타이머 진행 상태. 팝업이 닫혀있어도 시간은 계속 흘러야 하므로 별도 싱글톤으로 분리
public class CookingTimerController : MMSingleton<CookingTimerController>
{
    public bool IsCooking { get; private set; }
    public float RemainingTime { get; private set; }
    public float TotalTime { get; private set; }
    public string CookingRecipeId { get; private set; }
    public int CookingCount { get; private set; }

    private void Update()
    {
        if (!IsCooking) return;

        RemainingTime -= Time.deltaTime;

        if (RemainingTime <= 0f)
            CompleteCooking();
    }

    public bool StartCooking(string recipeId, int count, float cookingTimePerUnit)
    {
        if (IsCooking) return false; // 이미 조리 중이면 무시 (동시 조리는 요구사항에 없음)

        CookingRecipeId = recipeId;
        CookingCount = count;
        TotalTime = cookingTimePerUnit * count;
        RemainingTime = TotalTime;
        IsCooking = true;

        // 여기서 재료 소모 처리 필요
        // CookingManager.Instance.ConsumeIngredients(recipeId, count);

        EventManager.Instance.PostNotification(EventType.OnCookingStarted, this);
        return true;
    }

    public void CancelCooking()
    {
        if (!IsCooking) return;

        IsCooking = false;
        
        // 재료는 환불하지 않음 — 시작 시점에 이미 소모된 것으로 간주
        //취소 시 추가 처리 필요하면 여기에

        EventManager.Instance.PostNotification(EventType.OnCookingCanceled, this);
    }

    private void CompleteCooking()
    {
        IsCooking = false;

        // 완성품 지급 처리 필요
        // CookingManager.Instance.GrantCookedFood(CookingRecipeId, CookingCount);

        EventManager.Instance.PostNotification(EventType.OnCookingCompleted, this);
    }
}