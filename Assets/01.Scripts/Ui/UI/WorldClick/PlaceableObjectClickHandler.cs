using UnityEngine;

public class PlaceableObjectClickHandler : MonoBehaviour, IClickable
{
    public void OnClicked()
    {
        HousingStateController.Instance.HandleObjectClicked(gameObject);
    }
}