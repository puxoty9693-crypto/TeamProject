using UnityEngine;

public class DimController : MonoBehaviour
{
    [SerializeField] GameObject fullDim;
    [SerializeField] GameObject unlockDim;
    [SerializeField] GameObject recipeDim;
    [SerializeField] GameObject cookingDim;
    [SerializeField] GameObject housingDim;
    [SerializeField] GameObject breaktimeDim;

    public void OpenFull() => OpenDim(fullDim);
    public void CloseFull() => CloseDim(fullDim);
    public void OpenUnlock() => OpenDim(unlockDim);
    public void CloseUnlock() => CloseDim(unlockDim);

    public void OpenRecipel() => OpenDim(recipeDim);
    public void CloseRecipel() => CloseDim(recipeDim);
    public void OpenCooking() => OpenDim(cookingDim);
    public void CloseCooking() => CloseDim(cookingDim);
    public void OpenHousing() => OpenDim(housingDim);
    public void CloseHousing() => CloseDim(housingDim);
    public void Openbreaktime() => OpenDim(breaktimeDim);
    public void Closebreaktime() => CloseDim(breaktimeDim);


    public void OpenDim(GameObject dim)
    {
        dim.SetActive(true);
    }
    public void CloseDim(GameObject dim)
    {
        dim.SetActive(false);
    }





}
