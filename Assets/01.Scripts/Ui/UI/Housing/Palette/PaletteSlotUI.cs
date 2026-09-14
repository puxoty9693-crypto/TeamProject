using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaletteSlotUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Button selectButton;
    [SerializeField] TextMeshProUGUI countText;

    private string objId;

    private void Awake()
    {
        selectButton.onClick.AddListener(() => HousingSystem.Instance.SelectObjectToPlace(objId));
    }

    public void Setup(string id, Sprite iconSprite, int current, int max)
    {
        objId = id;
        icon.sprite = iconSprite;
        countText.text = $"{current}/{max}";
    }
}