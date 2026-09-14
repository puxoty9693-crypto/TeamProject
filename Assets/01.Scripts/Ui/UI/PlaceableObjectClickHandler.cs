using UnityEngine;

public class PlaceableObjectClickHandler : MonoBehaviour
{
    public void OnClicked()
    {
        HousingStateController.Instance.HandleObjectClicked(gameObject);
    }
}