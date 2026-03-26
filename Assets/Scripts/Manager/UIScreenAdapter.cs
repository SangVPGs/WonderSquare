using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIScreenAdapter : MonoBehaviour
{
    public CanvasScaler scaler;
    RectTransform rect;

    public float targetWidth = 1080f;
    public float targetHeight = 1920f;

    int lastW, lastH;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        Apply();
    }

    void Update()
    {
        if (Screen.width != lastW || Screen.height != lastH)
        {
            Apply();
        }
    }

    void Apply()
    {
        lastW = Screen.width;
        lastH = Screen.height;

        bool isLandscape = Screen.width > Screen.height;

        if (isLandscape)
        {
            // Scale theo HEIGHT
            scaler.matchWidthOrHeight = 1f;

            // Đưa frame về giữa
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);

            // Fix cứng 1080x1920
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
        }
        else
        {
            scaler.matchWidthOrHeight = 0f;

            // Full stretch
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(1, 1);

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}