//using UnityEngine;
//using UnityEngine.UI;
//
//public class OptionPanel : MonoBehaviour
//{
//    public Button closeBtn;
//    public Slider bgmSlider;
//    public Slider sfxSlider;
//
//
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        bgmSlider.onValueChanged.AddListener(BGMVolumeChange);
//        sfxSlider.onValueChanged.AddListener(SFXVolumeChange);
//        closeBtn.onClick.AddListener(PopupManager.instance.CloseOptionBtn);
//    }
//    private void OnEnable()
//    {
//        bgmSlider.value = SoundManager.instance.GetBgmVolume();
//        sfxSlider.value = SoundManager.instance.GetSFXVolume();
//    }
//
//    public void BGMVolumeChange(float vol)
//    {
//        SoundManager.instance.SetBgmVolume(vol);
//    }
//    public void SFXVolumeChange(float vol)
//    {
//        SoundManager.instance.SetSFXVolume(vol);
//    }
//}
