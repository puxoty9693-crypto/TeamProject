using System;
using System.Collections.Generic;
using UnityEngine;

public enum BgmType 
{
    MainTheme,
    Event
}



[System.Serializable]
public class BgmEntry 
{
    public BgmType type;
    public AudioClip clip;
}



[CreateAssetMenu(fileName = "BgmData", menuName = "Data/Bgm")]
public class BgmData : ScriptableObject
{
    public List<BgmEntry> bgmList;
}
