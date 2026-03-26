using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TMP_Dropdown musicDropdown;

    private void OnEnable()
    {
        // load value
        musicSlider.value = SettingsManager.MusicVolume;
        sfxSlider.value = SettingsManager.SfxVolume;
        musicDropdown.value = SettingsManager.MusicIndex;

        // listen
        musicSlider.onValueChanged.AddListener(OnMusicVolume);
        sfxSlider.onValueChanged.AddListener(OnSfxVolume);
        musicDropdown.onValueChanged.AddListener(OnMusicChanged);
    }

    private void OnDisable()
    {
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
        musicDropdown.onValueChanged.RemoveAllListeners();
    }

    private void OnMusicVolume(float value)
    {
        SettingsManager.MusicVolume = value;
    }

    private void OnSfxVolume(float value)
    {
        SettingsManager.SfxVolume = value;
    }

    private void OnMusicChanged(int index)
    {
        SettingsManager.MusicIndex = index;
    }
}