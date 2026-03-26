using UnityEngine;
using System;

public static class HistoryManager
{
    private const string KEY = "GAME_HISTORY";

    public static void AddRecord(int score)
    {
        GameHistory history = LoadHistory();

        GameRecord record = new GameRecord
        {
            score = score,
            date = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
        };

        history.records.Add(record);

        SaveHistory(history);
    }

    public static GameHistory LoadHistory()
    {
        if (!PlayerPrefs.HasKey(KEY))
            return new GameHistory();

        string json = PlayerPrefs.GetString(KEY);
        return JsonUtility.FromJson<GameHistory>(json);
    }

    private static void SaveHistory(GameHistory history)
    {
        string json = JsonUtility.ToJson(history);
        PlayerPrefs.SetString(KEY, json);
        PlayerPrefs.Save();
    }
}