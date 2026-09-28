using UnityEngine;

public class UpgradeTabController : MonoBehaviour
{
    [SerializeField] GameObject ingredientTabPanel; // 재료업그레이드 패널
    [SerializeField] GameObject staffTabPanel;       // 직원업그레이드 패널
    [SerializeField] GameObject adTabPanel;          // 광고업그레이드 패널

    public void ShowIngredientTab() => SetTab(ingredientTabPanel);
    public void ShowStaffTab() => SetTab(staffTabPanel);
    public void ShowAdTab() => SetTab(adTabPanel);

    private void SetTab(GameObject target)
    {
        ingredientTabPanel.SetActive(target == ingredientTabPanel);
        staffTabPanel.SetActive(target == staffTabPanel);
        adTabPanel.SetActive(target == adTabPanel);
    }

    private void OnEnable()
    {
        ShowIngredientTab(); // 팝업 열릴 때 기본 탭
    }
}
