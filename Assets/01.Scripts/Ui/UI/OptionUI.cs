using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void OnEnable()
    {
        bgmSlider.SetValueWithoutNotify(SoundManager.Instance.bgmVolume);
        sfxSlider.SetValueWithoutNotify(SoundManager.Instance.sfxVolume);

        bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
    }
    private void OnBgmVolumeChanged(float value)
    {
        SoundManager.Instance.SetBgmVolume(value);
    }

    private void OnSfxVolumeChanged(float value)
    {
        SoundManager.Instance.SetSfxVolume(value);
    }

    private void OnDisable()
    {
        bgmSlider.onValueChanged.RemoveListener(OnBgmVolumeChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
    }
}
