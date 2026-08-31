using UnityEngine;
// 나중에 전부 연결해서 사용해야함

public class PopupManager : MMSingleton<PopupManager>
{
    [SerializeField] GameObject dim;
    [SerializeField] GameObject chestPopup;

    public void OpenChest()
    {
        OpenPop(chestPopup);
    }
    public void CloseChest()
    {
        ClosePop(chestPopup);
    }








    #region 공유함수
    private void OpenPop(GameObject pop)
    {
        dim.SetActive(true);
        pop.SetActive(true);
    }
    private void ClosePop(GameObject pop)
    {
        dim.SetActive(false);
        pop.SetActive(false);
    }
    #endregion
}
