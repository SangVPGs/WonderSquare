using UnityEngine;
using UnityEngine.UI;

public class BottomPanelToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;

    private void Awake()
    {
        if (toggle == null)
            toggle = GetComponent<Toggle>();

        // set giá trị ban đầu
        toggle.isOn = SettingsManager.ShowBottomPanel;

        toggle.onValueChanged.AddListener(OnChanged);
    }

    private void OnEnable()
    {
        // sync lại khi mở setting
        toggle.isOn = SettingsManager.ShowBottomPanel;
    }

    private void OnChanged(bool value)
    {
        AudioManager.Instance?.PlayButton();
        SettingsManager.ShowBottomPanel = value;
    }
}