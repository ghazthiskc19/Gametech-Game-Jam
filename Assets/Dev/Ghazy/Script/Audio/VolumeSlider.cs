using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    public GameObject settings;
    public Slider[] allSliders;
    void Start()
    {
        allSliders = settings.GetComponentsInChildren<Slider>();

        allSliders[0].value = AudioManager.instance.GetBGMVolume();
        allSliders[1].value = AudioManager.instance.GetSFXVolume();

        allSliders[0].onValueChanged.AddListener(AudioManager.instance.SetMusicVolume);
        allSliders[1].onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);
    }
}
