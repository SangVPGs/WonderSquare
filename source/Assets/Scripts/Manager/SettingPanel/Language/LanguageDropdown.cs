using TMPro;
using UnityEngine;

public class LanguageDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private void Awake()
    {
        if (dropdown == null)
            dropdown = GetComponent<TMP_Dropdown>();

        // set giá trị ban đầu
        dropdown.value = (int)SettingsManager.CurrentLanguage;
        dropdown.RefreshShownValue();

        dropdown.onValueChanged.AddListener(OnChanged);
    }

    private void OnChanged(int index)
    {
        AudioManager.Instance?.PlayButton();
        SettingsManager.CurrentLanguage = (Language)index;
    }

    private void OnEnable()
    {
        // sync lại khi mở setting
        dropdown.value = (int)SettingsManager.CurrentLanguage;
        dropdown.RefreshShownValue();
    }
}