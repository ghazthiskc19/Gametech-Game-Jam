using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    void Start()
    {
        bgmSlider.value = AudioManager.instance.GetBGMVolume();
        sfxSlider.value = AudioManager.instance.GetSFXVolume();

        bgmSlider.onValueChanged.AddListener(AudioManager.instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);
    }
}
