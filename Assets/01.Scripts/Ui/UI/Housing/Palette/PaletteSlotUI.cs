using UnityEngine;
using UnityEngine.UI;

public class PaletteSlotUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Button selectButton;

    private string objId;

    private void Awake()
    {
        selectButton.onClick.AddListener(() => HousingSystem.Instance.SelectObjectToPlace(objId));
    }

    public void Setup(string id, Sprite iconSprite)
    {
        objId = id;
        icon.sprite = iconSprite;
    }
}