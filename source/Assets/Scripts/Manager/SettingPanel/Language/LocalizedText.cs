using TMPro;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string key;

    private TMP_Text text;

    private static Dictionary<string, string> en = new Dictionary<string, string>()
    {
        { "play", "Play" },
        { "settings", "Settings" },
        { "exit", "Exit" },
        { "continue", "Continue" },
        { "score", "Score" },
        { "final", "Final Score" },
        { "hold", "Hold" },
        { "next", "Next" },
        { "gameover", "Game Over" },
        { "replay", "Replay" },

        { "history", "History" },
        { "index", "Index" },
        { "time", "Time" },

        { "musictitle", "Music" },
        { "volumetitle", "Volume" },
        { "backgroundmusic", "Background" },
        { "check", "Turn on move buttons" },

        { "language", "Language" },

        { "guide", "Guide" },
        { "guidetext", "FOR WINDOW\r\n\r\n- Left, right, and down arrow keys to Move\r\n- Up arrow key to right Rotate\r\n- Z key to left Rotate\r\n- Space key to Drop\r\n- C key to Hold\r\n- S key to Open Settings\r\n\r\nFOR ANDROID\r\n\r\n- Swipe to Move and Drop" },

        { "quest", "Do you want to quit?" },
        { "notification", "Your current score will be saved" }
    };

    private static Dictionary<string, string> vi = new Dictionary<string, string>()
    {
        { "play", "Chơi" },
        { "settings", "Cài đặt" },
        { "exit", "Thoát" },
        { "continue", "Tiếp tục" },
        { "score", "Điểm" },
        { "final", "Điểm cuối" },
        { "hold", "Giữ" },
        { "next", "Tiếp theo" },
        { "gameover", "Trò chơi kết thúc" },
        { "replay", "Chơi lại" },

        { "history", "Lịch sử chơi" },
        { "index", "STT" },
        { "time", "Thời gian" },

        { "musictitle", "Âm nhạc" },
        { "volumetitle", "Âm lượng" },
        { "backgroundmusic", "Nền" },
        { "check", "Bật nút di chuyển" },

        { "language", "Ngôn ngữ" },

        { "guide", "Hướng dẫn" },
        { "guidetext", "DÀNH CHO MÁY TÍNH\r\n\r\n- Phím mũi tên trái, phải, xuống để di chuyển\r\n- Phím mũi tên lên để xoay phải\r\n- Phím Z để xoay trái\r\n- Phím Space để thả\r\n- Phím C để giữ\r\n- Phím S để mở cài đặt\r\n\r\nDÀNH CHO ANDROID\r\n\r\n- Vuốt để di chuyển và thả" },

        { "quest", "Bạn muốn thoát game?" },
        { "notification", "Điểm hiện tại của bạn sẽ được lưu lại" }
    };

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        SettingsManager.OnLanguageChanged += UpdateText;
        UpdateText();
    }

    private void OnDisable()
    {
        SettingsManager.OnLanguageChanged -= UpdateText;
    }

    private void UpdateText()
    {
        text.text = Get(key);
    }

    private string Get(string key)
    {
        var dict = SettingsManager.CurrentLanguage == Language.Vietnamese ? vi : en;

        return dict.TryGetValue(key, out var value) ? value : key;
    }
}