using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public Slider bgmSlider;
    [SerializeField] Button exitbutton;
    private void OnEnable()
    {
        bgmSlider.SetValueWithoutNotify(SoundManager.Instance.bgmVolume);
        bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        bgmSlider.value = SoundManager.Instance.bgmVolume;

        exitbutton.onClick.AddListener (() => SaveManager.Instance.ExitGame());
    }
    private void OnBgmVolumeChanged(float value)
    {
        SoundManager.Instance.SetBgmVolume(value);
    }


    private void OnDisable()
    {
        bgmSlider.onValueChanged.RemoveListener(OnBgmVolumeChanged);
    }
}

  //public Slider sfxSlider;
  //  private void OnSfxVolumeChanged(float value)
  //  {
  //      SoundManager.Instance.SetSfxVolume(value);
  //  }
  //sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
  // sfxSlider.SetValueWithoutNotify(SoundManager.Instance.sfxVolume);
  //sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);