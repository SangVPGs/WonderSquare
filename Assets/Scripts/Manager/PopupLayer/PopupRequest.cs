using UnityEngine;

public struct PopupRequest
{
    public string text;
    public int level;

    public PopupRequest(string text, int level, Vector2? position = null)
    {
        this.text = text;
        this.level = level;
    }
}