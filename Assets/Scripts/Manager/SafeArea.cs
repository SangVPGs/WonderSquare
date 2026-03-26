using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        Apply();
    }

    void Apply()
    {
        Rect safe = Screen.safeArea;

        Vector2 min = new Vector2(
            safe.xMin / Screen.width,
            safe.yMin / Screen.height
        );

        Vector2 max = new Vector2(
            safe.xMax / Screen.width,
            safe.yMax / Screen.height
        );

        rect.anchorMin = min;
        rect.anchorMax = max;

        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}