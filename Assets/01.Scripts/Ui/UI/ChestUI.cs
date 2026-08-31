using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour
{
    [SerializeField]List<ChestSlotUI> slots = new();
    [SerializeField] Button OpenBtn;
    [SerializeField] Button closeBtn;

    private void OnEnable()
    {
        SetSlots();
    }
    public void Start()
    {
        OpenBtn.onClick.AddListener(PopupManager.Instance.OpenChest);
        closeBtn.onClick.AddListener(PopupManager.Instance.CloseChest);
    }
    public void SetSlots()
    {
        List<IngredientStock> stocks = SaveManager.Instance.CurrentData.warehouseStock;

        for (int i = 0; i < stocks.Count; i++)
        {
            IngredientData data = DataManager.Instance.allIngredients.Find(x => x.ingredientId == stocks[i].ingredientId);
            if(data != null)
            slots[i].UdateChestSlotUI(data, stocks[i]);
        }
    }
    private void ClosePop()
    {
        this.gameObject.SetActive(false);
    }
}
