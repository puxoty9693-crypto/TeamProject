using UnityEngine;

public class PlaceableObjectClickHandler : MonoBehaviour
{
    public void OnClicked()
    {
        switch (HousingSystem.Instance.CurrentMode)
        {
            case HousingMode.Delete:
                HousingSystem.Instance.TryRemove(gameObject);
                break;

            case HousingMode.Move:
                HousingSystem.Instance.TryPickUp(gameObject);
                break;

            case HousingMode.Normal:
                break;
        }
    }
}