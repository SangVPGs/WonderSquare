using UnityEngine;
using System;

public enum Language
{
    English = 0,
    Vietnamese = 1
}

public static class SettingsManager
{
    public static event Action OnLanguageChanged;
    public static event Action OnBottomPanelChanged;

    // ================= AUDIO =================

    public static float MusicVolume
    {
        get => PlayerPrefs.GetFloat("MusicVolume", 1f);
        set
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            Apply();
        }
    }

    public static float SfxVolume
    {
        get => PlayerPrefs.GetFloat("SfxVolume", 1f);
        set
        {
            PlayerPrefs.SetFloat("SfxVolume", value);
            Apply();
        }
    }

    public static int MusicIndex
    {
        get => PlayerPrefs.GetInt("MusicIndex", 0);
        set
        {
            PlayerPrefs.SetInt("MusicIndex", value);
            Apply();
        }
    }

    // ================= LANGUAGE =================

    public static Language CurrentLanguage
    {
        get => (Language)PlayerPrefs.GetInt("Language", 0);
        set
        {
            PlayerPrefs.SetInt("Language", (int)value);
            PlayerPrefs.Save();

            OnLanguageChanged?.Invoke();
        }
    }

    // ================= BOTTOM PANEL =================

    public static bool ShowBottomPanel
    {
        get => PlayerPrefs.GetInt("ShowBottomPanel", 1) == 1;
        set
        {
            PlayerPrefs.SetInt("ShowBottomPanel", value ? 1 : 0);
            PlayerPrefs.Save();

            OnBottomPanelChanged?.Invoke();
        }
    }

    // ================= APPLY =================

    public static void Apply()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ApplySettings();
        }
    }
}